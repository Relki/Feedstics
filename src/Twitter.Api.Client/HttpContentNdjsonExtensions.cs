namespace Twitter.Api.Client
{
    using Newtonsoft.Json;

    /// <summary>
    /// HttpContent Extension methods.
    /// </summary>
    internal static class HttpContentNdjsonExtensions
    {
        /// <summary>
        /// Read stream lines as converted json classes.
        /// </summary>
        /// <typeparam name="TValue">Type json line is.</typeparam>
        /// <param name="contentStream">Stream content.</param>
        /// <returns>Async enumerable of type.</returns>
        /// <exception cref="ArgumentNullException">Stream content can not be null.</exception>
        public static async IAsyncEnumerable<TValue> ReadFromNdjsonAsync<TValue>(this Stream contentStream)
        {
            if (contentStream == null)
            {
                throw new ArgumentNullException(nameof(contentStream));
            }

            using (contentStream)
            {
                using var contentStreamReader = new StreamReader(contentStream);

                while (!contentStreamReader.EndOfStream)
                {
                    var content = await contentStreamReader.ReadLineAsync();
                    if (!string.IsNullOrWhiteSpace(content))
                    {
                        var tweet = JsonConvert.DeserializeObject<TValue>(content);
                        if (tweet != null)
                        {
                            yield return tweet;
                        }
                    }
                }
            }
        }
    }
}