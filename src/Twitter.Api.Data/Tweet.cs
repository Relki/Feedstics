namespace Twitter.Api.Data
{
    using Newtonsoft.Json;
    using System;
    using System.Diagnostics;

    /// <summary>
    /// Tweet object from Twitter API.
    /// </summary>
    /// <see cref="https://developer.twitter.com/en/docs/twitter-api/tweets/filtered-stream/api-reference/get-tweets-search-stream#tab1"/>
    [DebuggerDisplay($"{{{nameof(this.GetDebuggerDisplay)}(),nq}}")]
    public class Tweet
    {
        /// <summary>
        /// Tweet data.
        /// </summary>
        [JsonProperty("data")]
        public TweetData Data { get; set; } = default!;

        /// <summary>
        /// Additional fields included with API Query Parameters.
        /// </summary>
        [JsonProperty("includes")]
        public Includes? Includes { get; set; }

        /// <summary>
        /// Rules in a search stream this tweet matches.
        /// </summary>
        [JsonProperty("matching_rules")]
        public SearchStreamRule[]? MatchingRules { get; set; }

        private string GetDebuggerDisplay()
        {
            return this.ToString();
        }
    }

    /// <summary>
    /// Tweet data.
    /// </summary>
    public class TweetData
    {
        /// <summary>
        /// Unique identifier of this Tweet. This is returned as a string in order to avoid complications with languages and tools that cannot handle large integers.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; } = default!;

        /// <summary>
        /// The content of the Tweet.
        /// To return this field, add tweet.fields=text in the request's query parameter.
        /// </summary>
        [JsonProperty("text")]
        public string Text { get; set; } = default!;

        /// <summary>
        /// Creation time of the Tweet.
        /// To return this field, add tweet.fields=created_at in the request's query parameter.
        /// </summary>
        [JsonProperty("created_at")]
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// Unique identifier of this user. This is returned as a string in order to avoid complications with languages and tools that cannot handle large integers.
        /// You can obtain the expanded object in includes.users by adding expansions=author_id in the request's query parameter.
        /// </summary>
        [JsonProperty("author_id")]
        public string? AuthorId { get; set; }

        /// <summary>
        /// Unique identifiers indicating all versions of an edited Tweet.
        /// For Tweets with no edits, there will be one ID. For Tweets with
        /// an edit history, there will be multiple IDs, arranged in
        /// ascending order reflecting the order of edit, with the most
        /// recent version in the last position of the array.
        /// </summary>
        [JsonProperty("edit_history_tweet_ids")]
        public string[]? EditHistoryTweetIds { get; set; }

        /// <summary>
        /// Indicates if a Tweet is eligible for edit, how long it is editable for, and the number of remaining edits.
        /// 
        /// The is_edit_eligible field indicates if a Tweet was eligible for edit when published.
        /// This field is not dynamic and won't toggle from True to False when a Tweet reaches
        /// its editable time limit, or maximum number of edits. The following Tweet features will
        /// cause this field to be false:
        /// 
        /// Tweet is promoted
        /// Tweet has a poll
        /// Tweet is a non-self-thread reply
        /// Tweet is a Retweet(note that Quote Tweets are eligible for edit)
        /// Tweet is nullcast
        /// Community Tweet
        /// Superfollow Tweet
        /// 
        /// Editable Tweets can be edited for the first 30 minutes after creation and can be edited up to five times.
        /// 
        /// To return this field, add tweet.fields=edit_controls in the request's query parameter.
        /// </summary>
        [JsonProperty("edit_controls")]
        public EditControls? EditControls { get; set; }

        /// <summary>
        /// The Tweet ID of the original Tweet of the conversation (which includes direct replies, replies of replies).
        /// To return this field, add tweet.fields=conversation_id in the request's query parameter.
        /// </summary>
        [JsonProperty("conversation_id")]
        public string? ConversationId { get; set; }

        /// <summary>
        /// The Twitter screen name, handle, or alias that this user identifies themselves with.
        /// Usernames are unique but subject to change. Typically a maximum of 15 characters long,
        /// but some historical accounts may exist with longer names.
        /// </summary>
        [JsonProperty("username")]
        public string? Username { get; set; }

        /// <summary>
        /// Contains details about text that has a special meaning in a Tweet.
        /// To return this field, add tweet.fields=entities in the request's query parameter.
        /// </summary>
        [JsonProperty("entities")]
        public Entities? Entities {get;set;}
    }

    /// <summary>
    /// Contains details about text that has a special meaning in a Tweet.
    /// To return this field, add tweet.fields=entities in the request's query parameter.
    /// </summary>
    public class Entities
    {
        /// <summary>
        /// Contains details about annotations relative to the text within a Tweet.
        /// </summary>
        [JsonProperty("annotations")]
        public Annotations[]? Annotations { get; set; }

        /// <summary>
        /// Contains details about text recognized as a Hashtag.
        /// </summary>
        [JsonProperty("hashtags")]
        public Hashtag[]? Hashtags { get; set; }
    }

    /// <summary>
    /// Contains details about annotations relative to the text within a Tweet.
    /// </summary>
    public class Annotations
    {
        /// <summary>
        /// The start position (zero-based) of the text used to annotate the Tweet. All start indices are inclusive.
        /// </summary>
        [JsonProperty("start")]
        public int Start { get; set; }

        /// <summary>
        /// The end position (zero based) of the text used to annotate the Tweet. While all other end indices are exclusive, this one is inclusive.
        /// </summary>
        [JsonProperty("end")]
        public int End { get; set; }

        /// <summary>
        /// The confidence score for the annotation as it correlates to the Tweet text.
        /// </summary>
        [JsonProperty("probability")]
        public float Probability { get; set; }

        /// <summary>
        /// The description of the type of entity identified when the Tweet text was interpreted.
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; } = default!;

        /// <summary>
        /// The text used to determine the annotation type.
        /// </summary>
        [JsonProperty("normalized_text")]
        public string NormalizedText { get; set; } = default!;
    }

    /// <summary>
    /// Contains details about text recognized as a Hashtag.
    /// </summary>
    public class Hashtag
    {
        /// <summary>
        /// The start position (zero-based) of the recognized Hashtag within the Tweet. All start indices are inclusive.
        /// </summary>
        [JsonProperty("start")]
        public int Start { get; set; }

        /// <summary>
        /// The end position (zero-based) of the recognized Hashtag within the Tweet. This end index is exclusive.
        /// </summary>
        [JsonProperty("end")]
        public int End { get; set; }

        /// <summary>
        /// The text of the Hashtag.
        /// </summary>
        [JsonProperty("tag")]
        public string Tag { get; set; } = default!;
    }

    /// <summary>
    /// Tweet editable controls.
    /// </summary>
    public class EditControls
    {
        /// <summary>
        /// Tween is elgible to be edited.
        /// </summary>
        [JsonProperty("is_edit_eligible")]
        public bool IsEditEligible { get; set; }

        /// <summary>
        /// Max date this tweet is elgible to be edited.
        /// </summary>
        [JsonProperty("editable_until")]
        public DateTime EditableUntil { get; set; }

        /// <summary>
        /// Number of edits this tween can be edted.
        /// </summary>
        [JsonProperty("edits_remaining")]
        public int EditsRemaining { get; set; }
    }

    /// <summary>
    /// Additional data included for a tweet through Query Properties.
    /// </summary>
    public class Includes
    {
        [JsonProperty("users")]
        public User[]? Users { get; set; }
    }

    /// <summary>
    /// User data included for a tweet through Query Properties.
    /// </summary>
    public class User
    {
        /// <summary>
        /// The unique identifier of this user.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; } = default!;

        /// <summary>
        /// The name of the user, as they’ve defined it on their profile.
        /// Not necessarily a person’s name. Typically capped at 50 characters, but subject to change.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; } = default!;

        /// <summary>
        /// The Twitter screen name, handle, or alias that this user identifies themselves with.
        /// Usernames are unique but subject to change. Typically a maximum of 15 characters long,
        /// but some historical accounts may exist with longer names.
        /// </summary>
        [JsonProperty("username")]
        public string Username { get; set; } = default!;
    }
}
