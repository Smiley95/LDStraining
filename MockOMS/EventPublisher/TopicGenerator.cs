using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using Accounts.Domain.Events;
using Accounts.Domain.Events.Public;
using Linedata.Foundation.EventPublisher;

namespace EventPublisher
{
    public class TopicGenerator : ITopicGenerator
    {
        readonly IImmutableList<string> _onlyRootTopic = ImmutableArray.Create(@"\");

        public IImmutableList<string> GetTopics(object message)
        {
            return message switch
            {
                AccountCreated evt => evt.GetTopics(),
                AccountCredited evt => evt.GetTopics(),
                AccountDebited evt => evt.GetTopics(),

                _ => _onlyRootTopic
            };
        }
    }
}
