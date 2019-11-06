using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KnightBus.Core;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Queue.Protocol;

namespace KnightBus.Azure.Storage
{
    

    public class StorageBusTransportManager : ITransportManager
    {
        private readonly IStorageBusConfiguration _configuration;

        public StorageBusTransportManager(IStorageBusConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<IList<string>> ListQueues(CancellationToken cancellationToken)
        {
            var storage = CloudStorageAccount.Parse(_configuration.ConnectionString);
            var queueClient = storage.CreateCloudQueueClient();
            QueueContinuationToken token = null;
            var queues = new List<string>();
            do
            {
                var result = await queueClient.ListQueuesSegmentedAsync(string.Empty, QueueListingDetails.None, null, token, null, null, cancellationToken).ConfigureAwait(false);
                token = result.ContinuationToken;
                var listedQueues = result.Results.ToList();
                if (listedQueues.Any())
                {
                    queues.AddRange(listedQueues.Select(queue => queue.Name));
                }

            } while (token != null);

            return queues;
        }
    }
}