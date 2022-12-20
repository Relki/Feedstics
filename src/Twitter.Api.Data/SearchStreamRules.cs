namespace Twitter.Api.Data
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    /// <summary>
    /// Search stream rules to add or delete. These rules affect tweets returned in a search stream.
    /// </summary>
    public class SearchStreamRules
    {
        /// <summary>
        /// Gets or sets rules to be added to the search stream.
        /// </summary>
        /// <remarks>
        /// Number of rules allowed depend on the developer app plan tier.
        /// </remarks>
        [JsonProperty("add", NullValueHandling = NullValueHandling.Ignore)]
        public List<SearchStreamRule>? Add { get; set; }

        /// <summary>
        /// Gets or sets rules to be deleted from the search stream.
        /// </summary>
        [JsonProperty("delete", NullValueHandling = NullValueHandling.Ignore)]
        public List<string>? Delete { get; set; }
    }
}
