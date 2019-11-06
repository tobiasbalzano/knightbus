using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace KnightBus.Core
{
    /// <summary>
    /// Defines a message transport
    /// </summary>
    public interface ITransport
    {
        ITransportChannelFactory[] TransportChannelFactories { get; }
        ITransport ConfigureChannels(ITransportConfiguration configuration);
        ITransport UseMiddleware(IMessageProcessorMiddleware middleware);
    }


    public interface ITransportManager
    {
        Task<IList<string>> ListQueues(CancellationToken cancellationToken);
    }

    public struct QueueDetails
    {
        public QueueDetails(string name, int messageCount, int deadLetterCount)
        {
            Name = name;
            MessageCount = messageCount;
            DeadLetterCount = deadLetterCount;
        }
        public string Name { get; }
        public int MessageCount { get; }
        public int DeadLetterCount { get; }
    }
}