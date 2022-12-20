namespace Feedstistics.Shared.Queue
{
    using Azure.Storage.Queues.Models;
    using System.Threading.Tasks;

    /// <summary>
    /// Queue client interface.
    /// </summary>
    public interface IQueueClient
    {
        /// <summary>
        /// Enqueue a message.
        /// </summary>
        /// <param name="message">Message object to enqueue.</param>
        /// <param name="cancellationToken">Optional: CancellationToken.</param>
        /// <returns>Task.</returns>
        public Task EnqueueMessageAsync(object message, CancellationToken cancellationToken = default);

        /// <summary>
        /// Dequeue a message.
        /// </summary>
        /// <param name="cancellationToken">Optional: CancellationToken.</param>
        /// <returns>Task.</returns>
        public Task<QueueMessage> DequeueMessageAsync(CancellationToken cancellationToken = default);
    }
}
