namespace Twitter.Api.Data
{
    using Newtonsoft.Json;
    using System.Diagnostics;

    /// <summary>
    /// Stream rule to apply to tweets returned from a search stream.
    /// </summary>
    [DebuggerDisplay($"{{{nameof(this.GetDebuggerDisplay)}(),nq}}")]
    public class SearchStreamRule
    {
        /// <summary>
        /// Gets or sets the rule text as submitted when creating the rule.
        /// </summary>
        /// <example>
        /// "(from:username coffee) has:media -is:retweet"
        /// "(grumpy \"cat"\) has:media is:retweet"
        /// </example>
        /// <see cref="https://developer.twitter.com/apitools/api?endpoint=%2F2%2Ftweets%2Fsearch%2Fstream%2Frules&method=post"/>
        [JsonProperty("value")]
        public string? Value { get; set; }

        /// <summary>
        /// Gets or sets the tag label as defined when creating the rule.
        /// </summary>
        [JsonProperty("tag")]
        public string? Tag { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of this rule. This is returned as a string in order to avoid complications with languages and tools that cannot handle large integers.
        /// </summary>
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string? Id { get; set; }

        private string GetDebuggerDisplay()
        {
            return this.ToString();
        }
    }
}