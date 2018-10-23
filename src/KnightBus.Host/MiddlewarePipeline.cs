using System.Collections.Generic;
using KnightBus.Core;
using KnightBus.Host.DefaultMiddlewares;

namespace KnightBus.Host
{
    internal class MiddlewarePipeline
    {
        private readonly List<IMessageProcessorMiddleware> _middlewares = new List<IMessageProcessorMiddleware>();

        public MiddlewarePipeline(IEnumerable<IMessageProcessorMiddleware> hostMiddlewares, ITransportConfiguration transportConfiguration, ILog log)
        {

            //Add default outlying middlewares
            _middlewares.Add(new ErrorHandlingMiddleware(log));
            _middlewares.Add(new DeadLetterMiddleware());
            //Add host-global middlewares
            _middlewares.AddRange(hostMiddlewares);
            //Add transport middlewares
            _middlewares.AddRange(transportConfiguration.Middlewares);
            
            if (transportConfiguration?.AttachmentProvider != null)
            {
                _middlewares.Add(new AttachmentMiddleware(transportConfiguration.AttachmentProvider));
            }
        }

        public IMessageProcessor GetPipeline(IMessageProcessor baseProcessor)
        {
            var processors = new IMessageProcessor[_middlewares.Count + 1];
            processors[processors.Length - 1] = baseProcessor;
            for (var i = processors.Length - 2; i >= 0; i--)
            {
                processors[i] = new MiddlewareWrapper(_middlewares[i], processors[i + 1]);
            }

            return processors[0];
        }
    }
}