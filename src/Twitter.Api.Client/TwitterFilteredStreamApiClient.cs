namespace Twitter.Api.Client
{
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Web;
    using Twitter.Api.Client.Data;
    using Twitter.Api.Data;

    /// <summary>
    /// Twitter Filtered Stream Api Client.
    /// </summary>
    public class TwitterFilteredStreamApiClient : OAuthBase, ITwitterFilteredStreamApiClient
    {
        private readonly ILogger<TwitterFilteredStreamApiClient> _logger;
        private TwitterClientSettings _settings;
        private readonly object _tweetCollectionLock = false;

        /// <inheritdoc/>
        public event EventHandler<StreamStartedEventArgs>? FilteredStream_Started;

        /// <inheritdoc/>
        public event EventHandler<StreamEndedEventArgs>? FilteredStream_Ended;

        /// <inheritdoc/>
        public event EventHandler<StreamFilteredTweetReceivedEventArgs>? FilteredStream_TweetsReceived;

        /// <inheritdoc/>
        public SearchStreamRules? CurrentStreamRules { get; set; } = null;

        /// <summary>
        /// Instansiates an instance of <see cref="TwitterFilteredStreamApiClient"/>
        /// </summary>
        /// <param name="loggerFactory">Logger.</param>
        /// <param name="settings">Settings.</param>
        public TwitterFilteredStreamApiClient(
            ILoggerFactory loggerFactory,
            IOptions<TwitterClientSettings> settings)
        {
            this._logger = loggerFactory.CreateLogger<TwitterFilteredStreamApiClient>();
            this._settings = settings.Value;

            if (settings.Value != null && string.IsNullOrWhiteSpace(settings.Value.AccessToken))
            {
                this._settings.AccessToken = this.GetBearerToken().Result;
            }
        }

        /// <inheritdoc/>
        public async Task<SearchStreamRules?> GetTweetStreamRules()
        {
            var route = "2/tweets/search/stream/rules";

            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(this._settings.TwitterBaseUrl)
            };

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{this._settings.AccessToken}");

            try
            {
                using var response = await httpClient.GetAsync(route);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<SearchStreamRules>(responseContent);
            }
            catch (Exception ex)
            {
                this._logger.LogCritical(ex, "Exception occured while getting response from twitter api.");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<SearchStreamResponse?> SetTweetStreamRulesAsync(bool? dryRun = null, SearchStreamRules? searchStreamRules = null)
        {
            var streamRules = searchStreamRules ?? this._settings.SearchStreamRules;

            if (streamRules == null)
            {
                throw new InvalidOperationException("Stream Rules must be supplied or specifed in settings.");
            }

            var dryRunQueryParameterString = dryRun == null ? string.Empty : $"?dry_run={dryRun}";
            var route = $"/2/tweets/search/stream/rules{dryRunQueryParameterString}";

            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(this._settings.TwitterBaseUrl)
            };

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{this._settings.AccessToken}");
            var contentSerialized = JsonConvert.SerializeObject(streamRules);
            var content = new StringContent(contentSerialized, Encoding.UTF8, "application/json");

            try
            {
                using var response = await httpClient.PostAsync(route, content);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<SearchStreamResponse>(responseContent);
            }
            catch (Exception ex)
            {
                this._logger.LogCritical(ex, "Exception occured while setting twitter stream rules in twitter api.");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task StartTweetStreamer(SearchStreamRules? searchStreamRules = null, CancellationToken? cancellationtoken = default!)
        {
            this.OnFilteredStreamStarted(new StreamStartedEventArgs());

            try
            {
                var existingSearchStreamRules = await this.GetTweetStreamRules();
                if ((existingSearchStreamRules == null || existingSearchStreamRules.Add == null))
                {
                    await this.SetTweetStreamRulesAsync(searchStreamRules: searchStreamRules);
                }
                else if (searchStreamRules == null)
                {
                    if (this._settings.SearchStreamRules != null && this._settings.SearchStreamRules.Add != null && existingSearchStreamRules.Add != null)
                    {
                        if (existingSearchStreamRules.Add.All(existingRule => this._settings.SearchStreamRules.Add.Any(settingRule => settingRule.Tag == existingRule.Tag)))
                        {
                            await this.SetTweetStreamRulesAsync();
                        }
                    }
                }
                else
                {
                    if (this._settings.SearchStreamRules != null && this._settings.SearchStreamRules.Add != null && searchStreamRules.Add != null)
                    {
                        if (searchStreamRules.Add.All(existingRule => this._settings.SearchStreamRules.Add.Any(settingRule => settingRule.Tag == existingRule.Tag)))
                        {
                            await this.SetTweetStreamRulesAsync();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.OnFilteredStreamEnded(new StreamEndedEventArgs(StoppedReason.Exception));
                throw;
            }

            var unprocessedTweets = new List<Tweet>();

            // If using batching by interval, timer will fire ever interval and gather up tweets and fire them in event.
            var batchTimer = new Timer(state =>
            {
                var batchedTweets = GetBatchedTweets(unprocessedTweets);
                //if (state is Tweet[] batchedTweets)
                //{
                if (batchedTweets != null && batchedTweets.Length > 0)
                {
                    this.OnFilteredStreamTweetsReceived(new StreamFilteredTweetReceivedEventArgs(batchedTweets));
                }
                //}
            }, null, this._settings.UseTweetStreamBatchingByInterval ? this._settings.TweetStreamBatchingMilliseconds : Timeout.Infinite, this._settings.TweetStreamBatchingMilliseconds);


            // Replace query parameters with a query parameter builder if we expect we'll want to change the data coming in.
            var route = "/2/tweets/search/stream?tweet.fields=author_id,conversation_id,created_at,text,entities&expansions=author_id&user.fields=username";

            try
            {
                using var httpClient = new HttpClient
                {
                    BaseAddress = new Uri(this._settings.TwitterBaseUrl),
                    Timeout = TimeSpan.FromMilliseconds(Timeout.Infinite),
                };

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{this._settings.AccessToken}");
                using var response = await httpClient.GetStreamAsync(route);

                await foreach (var tweet in response.ReadFromNdjsonAsync<Tweet>())
                {
                    lock (this._tweetCollectionLock)
                    {
                        unprocessedTweets.Add(tweet);
                        // If batching by quantity, and quantity size is met, batch up tweets and fire event.
                        if (this._settings.UseTweetStreamBatchingByQuantity && unprocessedTweets.Count >= this._settings.TweetStreamBatchingSize)
                        {
                            BatchTweetsByQuantity(unprocessedTweets);
                        }
                    }

                    if (cancellationtoken.HasValue && cancellationtoken.Value.IsCancellationRequested)
                    {
                        batchTimer.Change(Timeout.Infinite, this._settings.TweetStreamBatchingMilliseconds);
                        batchTimer.Dispose();

                        response.Close();

                        // Flush remaining tweets;
                        if (this._settings.UseTweetStreamBatchingByQuantity)
                        {
                            while (unprocessedTweets.Count > 0)
                            {
                                lock (_tweetCollectionLock)
                                {
                                    this.BatchTweetsByQuantity(unprocessedTweets);
                                }
                            }
                        }
                        else
                        {
                            this.OnFilteredStreamTweetsReceived(new StreamFilteredTweetReceivedEventArgs(this.GetBatchedTweets(unprocessedTweets)));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this._logger.LogCritical(ex, "Exception occured while reading search stream from twitter api.");
                this.OnFilteredStreamEnded(new StreamEndedEventArgs(StoppedReason.Exception));
                throw;
            }
        }

        /// <inheritdoc/>
        public void SetTweetBatchingMilliseconds(int batchingMilliseconds)
        {
            if (this._settings != null)
            {
                this._settings.TweetStreamBatchingMilliseconds = batchingMilliseconds;
            }
            else
            {
                this._settings = new TwitterClientSettings
                {
                    TweetStreamBatchingMilliseconds = batchingMilliseconds,
                };
            }
        }

        /// <inheritdoc/>
        public void SetTweetBatchingSize(int batchingSize)
        {
            if (this._settings != null)
            {
                this._settings.TweetStreamBatchingSize = batchingSize;
            }
            else
            {
                this._settings = new TwitterClientSettings
                {
                    TweetStreamBatchingSize = batchingSize,
                };
            }
        }

        protected virtual void OnFilteredStreamStarted(StreamStartedEventArgs e)
        {
            // Event listeners could stop listening between checking and invoking
            // so grab reference first before evaluating.
            var filteredStreamStartedEvent = this.FilteredStream_Started;
            if (filteredStreamStartedEvent != null)
            {
                filteredStreamStartedEvent?.Invoke(this, e);
            }
        }

        protected virtual void OnFilteredStreamEnded(StreamEndedEventArgs e)
        {
            // Event listeners could stop listening between checking and invoking
            // so grab reference first before evaluating.
            var filteredStreamEndedEvent = this.FilteredStream_Ended;
            if (filteredStreamEndedEvent != null)
            {
                filteredStreamEndedEvent?.Invoke(this, e);
            }
        }

        protected virtual void OnFilteredStreamTweetsReceived(StreamFilteredTweetReceivedEventArgs e)
        {
            // Event listeners could stop listening between checking and invoking
            // so grab reference first before evaluating.
            var filteredStreamTweetsReceivedEvent = this.FilteredStream_TweetsReceived;
            if (filteredStreamTweetsReceivedEvent != null)
            {
                filteredStreamTweetsReceivedEvent?.Invoke(this, e);
            }
        }

        private async Task<string?> GetBearerToken()
        {
            //https://dev.twitter.com/oauth/application-only
            //Step 1

            string strBearerRequest = HttpUtility.UrlEncode(this._settings.ConsumerKey) + ":" + HttpUtility.UrlEncode(this._settings.ConsumerSecret);
            strBearerRequest = Convert.ToBase64String(Encoding.UTF8.GetBytes(strBearerRequest));

            //Step 2

            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(this._settings.TwitterBaseUrl),
            };

            httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + strBearerRequest);

            var strRequestContent = new StringContent("grant_type=client_credentials");
            strRequestContent.Headers.ContentType = new MediaTypeHeaderValue(@"application/x-www-form-urlencoded");

            var response = await httpClient.PostAsync("/oauth2/token", strRequestContent);

            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();

            JObject jobjectResponse = JObject.Parse(content);

            if (jobjectResponse == null || jobjectResponse["access_token"] == null)
            {
                throw new InvalidOperationException("OAuth response was null.");
            }

            return jobjectResponse["access_token"].ToString();
        }

        private Tweet[] GetBatchedTweets(List<Tweet>? unprocessedTweets)
        {
            if (unprocessedTweets != null && unprocessedTweets.Count > 0)
            {
                var batchedTweets = unprocessedTweets.ToArray();

                lock (this._tweetCollectionLock)
                {
                    foreach (var tweet in batchedTweets)
                    {
                        unprocessedTweets.Remove(tweet);
                    }
                }

                return batchedTweets;
            }

            return Array.Empty<Tweet>();
        }

        private void BatchTweetsByQuantity(List<Tweet>? unprocessedTweets)
        {
            if (unprocessedTweets != null && unprocessedTweets.Count > 0)
            {
                var batchedTweets = unprocessedTweets.Take(this._settings.TweetStreamBatchingSize).ToArray();

                foreach (var tweet in batchedTweets)
                {
                    unprocessedTweets.Remove(tweet);
                }

                this.OnFilteredStreamTweetsReceived(new StreamFilteredTweetReceivedEventArgs(batchedTweets.ToArray()));
            }
        }
    }
}