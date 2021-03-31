using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Linedata.Foundation.Messaging;

namespace EventSubscriber
{
   sealed class ClientEventHandler : IEventHandler
    {
        public Task HandleAsync(EventContext context)
        {
            Console.WriteLine(
                "Received event on topic {3}: {0} (pos={1}, prev={2})",
                context.PayloadType.Name,
                context.Position,
                context.PreviousPosition,
                context.Topic);

            return Task.CompletedTask;
        }
    }
}
