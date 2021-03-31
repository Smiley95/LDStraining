using System;
using System.Collections.Generic;
using System.Text;
using Accounts.Domain.Events.Private;
using Accounts.Domain.Events;
using Linedata.Foundation.EventPublisher;

namespace EventPublisher
{
    public class EventToMessageTranslator : IEventTranslator
    {

        public bool Translate(object eventToTranslate, out object message)
        {
            Console.WriteLine("EventPublisher received event to translate: ", eventToTranslate);

            try
            {
                switch (eventToTranslate)
                {
                    case AccountCreated evt:
                        {
                            message = evt.ToPublic();
                            return true;
                        }

                    case AccountCredited evt:
                        {
                            message = evt.ToPublic();

                            return true;
                        }

                    case AccountDebited evt:
                        {
                            message = evt.ToPublic();

                            return true;
                        }
                    default:
                        {
                            message = null;
                            return false;
                        }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Could not translate Event from Private to Public: ", e);

                throw;
            }
        }
    }
}
