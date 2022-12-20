namespace Feedstistics.Data.Statistic
{
    using Newtonsoft.Json;

    /// <summary>
    /// Twitter Feed Annotation Count.
    /// </summary>
    public class TwitterFeedAnnotationCount : StatisticBase<int>
    {
        /// <summary>
        /// Instantiates an instance of <see cref="TwitterFeedAnnotationCount"/>.
        /// </summary>
        public TwitterFeedAnnotationCount()
            : base()
        {
        }

        /// <summary>
        /// Instantiates an instance of <see cref="TwitterFeedAnnotationCount"/> with an Statistic entity.
        /// </summary>
        public TwitterFeedAnnotationCount(EF.Models.Entity.Statistics statistic)
            : base(statistic)
        {
        }

        /// <summary>
        /// Instantiates an instance of <see cref="TwitterFeedAnnotationCount"/> with a serialized dimension.
        /// </summary>
        public TwitterFeedAnnotationCount(string serializedValueDimensions, DateTime sampleDateTimeUtc)
            : base(nameof(TwitterFeedAnnotationCount), serializedValueDimensions, sampleDateTimeUtc)
        {
        }

        /// <summary>
        /// Gets AnnotationType.
        /// </summary>
        [JsonIgnore]
        public string? AnnotationType
        {
            get
            {
                if (this.Dimensions != null && this.Dimensions.Count > 0 && this.Dimensions.TryGetValue("AnnotationType", out var annotationType))
                {
                    return annotationType?.ToString();
                }

                return default!;
            }
        }

        /// <summary>
        /// Gets AnnotationString.
        /// </summary>
        [JsonIgnore]
        public string? AnnotationString
        {
            get
            {
                if (this.Dimensions != null && this.Dimensions.Count > 0 && this.Dimensions.TryGetValue("AnnotationString", out var annotationString))
                {
                    return annotationString?.ToString();
                }

                return default!;
            }
        }
    }
}
