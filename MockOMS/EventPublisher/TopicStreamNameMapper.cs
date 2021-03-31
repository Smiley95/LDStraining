using System;
using System.Collections.Generic;
using System.Text;
using Linedata.Foundation.EventPublisher;
using Linedata.Foundation.Messaging;

namespace EventPublisher
{
    public class TopicStreamNameMapper : ITopicStreamNameMapper
    {
        const string streamPrefix = "gateway";

        public bool TryGetStreamName(Topic topic, out string stream)
        {
            var topicName = topic.ToString();

            if (topicName.StartsWith("\\FixConnections"))
            {
                stream = $"$ce-{streamPrefix}.FixConnectionAggregate";
                return true;
            }

            if (topicName.StartsWith("\\Accounts"))
            {
                stream = $"$ce-{streamPrefix}.Account";
                return true;
            }

            stream = null;
            return false;
        }
    }
}
