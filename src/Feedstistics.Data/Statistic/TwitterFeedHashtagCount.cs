namespace Feedstistics.Data.Statistic
{
    using Newtonsoft.Json;

    /// <summary>
    /// Twitter Feed Hashtag Count.
    /// </summary>
    public class TwitterFeedHashtagCount : StatisticBase<long>
    {
        /// <summary>
        /// Instantiates an instance of <see cref="TwitterFeedAvgTweetLength"/>.
        /// </summary>
        public TwitterFeedHashtagCount()
            : base()
        {
        }

        /// <summary>
        /// Instantiates an instance of <see cref="TwitterFeedAvgTweetLength"/> with an Statistic entity.
        /// </summary>
        public TwitterFeedHashtagCount(EF.Models.Entity.Statistics statistic)
            : base(statistic)
        {
        }

        /// <summary>
        /// Instantiates an instance of <see cref="TwitterFeedAvgTweetLength"/> with a serialized dimension.
        /// </summary>
        public TwitterFeedHashtagCount(string serializedValueDimensions, DateTime sampleDateTimeUtc)
            : base(nameof(TwitterFeedHashtagCount), serializedValueDimensions, sampleDateTimeUtc)
        {
        }

        /// <summary>
        /// Instantiates an instance of <see cref="TwitterFeedAvgTweetLength"/> with an existing statistic base fields.
        /// </summary>
        public TwitterFeedHashtagCount(IStatistic statistic)
            : base()
        {
            this.Name = nameof(TwitterFeedHashtagCount);
            this.SampleDateTime = statistic.SampleDateTime;
            this.Dimensions = statistic.Dimensions;
        }

        /// <summary>
        /// Gets Hashtag meta data for this statistic.
        /// </summary>
        [JsonIgnore]
        public string? Hashtag {
            get
            {
                if (this.Dimensions != null && this.Dimensions.Count > 0 && this.Dimensions.TryGetValue("Hashtag", out var hashtag))
                {
                    return hashtag?.ToString();
                }

                return default!;
            }
        }
    }
}
