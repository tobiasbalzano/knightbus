using System.Threading;
using System.Threading.Tasks;
using KnightBus.Core;

namespace KnightBus.Host.Tests.Unit.Processors
{
    public class AttachmentCommandProcessor : IProcessCommand<AttachmentCommand, TestMessageSettings>
    {
        public Task ProcessAsync(AttachmentCommand message, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}