namespace Twitter.Api.Client.Data
{
    using Newtonsoft.Json;
    using Twitter.Api.Data;

    /// <summary>
    /// Response from search stream rules retreival and setting.
    /// </summary>
    public class SearchStreamResponse
    {
        /// <summary>
        /// Response rules data.
        /// </summary>
        [JsonProperty("data")]
        public List<SearchStreamRule> SearchStreamRules { get; set; } = new List<SearchStreamRule>();

        /// <summary>
        /// Contains information about when the rule was created, and whether the rule was either created or not created, or deleted or not deleted.
        /// </summary>
        [JsonProperty("meta")]
        public MetaSummary? Meta { get; set; }

        /// <summary>
        /// Contains details about errors that affected any of the requested Tweets.
        /// See Status codes and error messages for more details.
        /// </summary>
        /// <see cref="https://developer.twitter.com/en/support/twitter-api/error-troubleshooting"/>
        [JsonProperty("errors")]
        public object? Errors { get; set; }
    }

    /// <summary>
    /// Contains information about when the rule was created, and whether the rule was either created or not created, or deleted or not deleted.
    /// </summary>
    public class MetaSummary
    {
        /// <summary>
        /// The time when the request body was returned.
        /// </summary>
        [JsonProperty("sent")]
        public string? Sent { get; set; }

        /// <summary>
        /// Contains fields that describe whether you were successful or unsuccessful in creating or deleting the different rules that you passed in your request.
        /// </summary>
        [JsonProperty("summary")]
        public Summary? Summary { get; set; }
    }

    public class Summary
    {
        /// <summary>
        /// Number of rules created.
        /// </summary>
        [JsonProperty("created")]
        public int Created { get; set; } = 0;

        /// <summary>
        /// Number of rules not created.
        /// </summary>
        [JsonProperty("not_created")]
        public int NotCreated { get; set; } = 0;

        /// <summary>
        /// Number of rules deleted.
        /// </summary>
        [JsonProperty("deleted")]
        public int Deleted { get; set; } = 0;

        /// <summary>
        /// Number of rules not deleted.
        /// </summary>
        [JsonProperty("not_deleted")]
        public int NotDeleted { get; set; } = 0;
    }

}
