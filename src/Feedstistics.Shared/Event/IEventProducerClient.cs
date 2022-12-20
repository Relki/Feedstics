namespace Feedstistics.Shared.Event
{
    /// <summary>
    /// Event Producer client interface.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IEventProducerClient<T>
    {
        /// <summary>
        /// Exception occured event.
        /// </summary>
        public event EventHandler<Exception>? ExceptionOccured;

        void Dispose();

        /// <summary>
        /// Send event to event producer client.
        /// </summary>
        /// <param name="message">Message to serialize and send.</param>
        /// <param name="continueOnException">Optional: True to continue on exception. <c>false</c> to throw. Defautl <c>false</c>.</param>
        /// <param name="cancellationToken">Optional: CancellationToken.</param>
        /// <returns>True if message sent successfully. False if not.</returns>
        public Task<bool> SendEvent(T message, bool? continueOnException = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// Send events to event producer client.
        /// </summary>
        /// <param name="message">Messages to serialize and send.</param>
        /// <param name="continueOnException">Optional: True to continue on exception. <c>false</c> to throw. Defautl <c>false</c>.</param>
        /// <param name="cancellationToken">Optional: CancellationToken.</param>
        /// <returns>True if all messages sent successfully. False if not.</returns>
        public Task<bool> SendEvents(IEnumerable<T> message, bool? continueOnException = false, CancellationToken cancellationToken = default);
    }
}
