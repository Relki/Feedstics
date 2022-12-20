namespace Feedstistics.Shared.Event
{
    /// <summary>
    /// Azure Event Hub Producer Client Settings.
    /// </summary>
    public class AzureEventHubProducerClientSettings
    {
        /// <summary>
        /// Gets or sets the Azure Event Hub Connection String.
        /// </summary>
        public string EventHubConnectionString { get; set; } = "https://my-event-hub-namespace.servicebus.windows.net:443/";

        /// <summary>
        /// Gets or sets the Azure Event Hub name.
        /// </summary>
        public string EventHubName { get; set; } = "my-event-hub-name";
    }
}
