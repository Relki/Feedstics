namespace Twitter.Api.Client
{
    using Twitter.Api.Data;

    /// <summary>
    /// Twitter API Client Settings.
    /// </summary>
    public class TwitterClientSettings
    {
        /// <summary>
        /// Gets or sets the base url to the twitter api.
        /// </summary>
        public string TwitterBaseUrl { get; set; } = "https://api.twitter.com";

        /// <summary>
        /// Gets or sets the access token used in in the twitter api for OAuth2 requirements.
        /// If left blank, an App Access Token will be requested from Twitter OAuth2 api using ConsumerKey and ConsumerSecret.
        /// </summary>
        public string? AccessToken { get; set; }

        /// <summary>
        /// Gets or sets the consumer Key to access Twitter API.
        /// </summary>
        public string? ConsumerKey { get; set; }

        /// <summary>
        /// Gets or sets the consumer Secret to access Twitter API.
        /// </summary>
        public string? ConsumerSecret { get; set; }

        /// <summary>
        /// Gets ors sets a value indicating if streamed tweets should be batched by quantity.
        /// If true, TweetStreamBatchingSize determines max tweets per tweets received event.
        /// </summary>
        public bool UseTweetStreamBatchingByQuantity { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicated if streamed tweets should be batched by a time interval.
        /// If true, TweetStreamBatchingMilliseconds will be used to determine intervals between batches sent.
        /// </summary>
        public bool UseTweetStreamBatchingByInterval { get; set; } = true;

        /// <summary>
        /// Gets or sets the milliseconds between batches of tweets being sent.
        /// Will only be used if UseTweetStreamBatchingInterval is true.
        /// </summary>
        public int TweetStreamBatchingMilliseconds { get; set; } = 1000;

        /// <summary>
        /// Gets or sets the batching size (number of tweets) for tweets being sent.
        /// Will only be used if UseTweetStreamBatchingQuantity is true.
        /// </summary>
        public int TweetStreamBatchingSize { get; set; } = 10;

        /// <summary>
        /// Gets or sets default set of SearchStreamRules to use.
        /// </summary>
        public SearchStreamRules? SearchStreamRules { get; set; }

        /// <summary>
        /// True if AccessToken is available.
        /// </summary>
        public bool TokenExist => !string.IsNullOrWhiteSpace(AccessToken); 
    }
}
