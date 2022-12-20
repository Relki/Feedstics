namespace Feedstistics.Shared.Queue
{
    /// <summary>
    /// Azure Storage Queue Client Settings.
    /// </summary>
    public class AzureStorageQueueClientSettings
    {
        /// <summary>
        /// Azure Queue connection string.
        /// </summary>
        public string AzureQueueConnectionString { get; set; } = "UseDevelopmentStorage=true";

        /// <summary>
        /// Azure queue name.
        /// </summary>
        public string AzureQueueName { get; set; } = "feedexport";
    }
}
