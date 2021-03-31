using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using Accounts.Domain.Events;
using Accounts.Domain.Events.Public;

namespace EventPublisher
{
    public static class TopicExtensions
    {
        public static IImmutableList<string> GetTopics(this AccountCreated self)
        {
            return ImmutableArray.CreateRange(
                new[]
                {
                    "\\Accounts",
                    $"\\Account\\{self.AccountId}",
                });
        }

        public static IImmutableList<string> GetTopics(this AccountCredited self)
        {
            return ImmutableArray.CreateRange(
                new[]
                {
                    "\\Accounts",
                    $"\\Account\\{self.AccountId}",
                });
        }

        public static IImmutableList<string> GetTopics(this AccountDebited self)
        {
            return ImmutableArray.CreateRange(
                new[]
                {
                    "\\Accounts",
                    $"\\Account\\{self.AccountId}",
                });
        }
    }
}
