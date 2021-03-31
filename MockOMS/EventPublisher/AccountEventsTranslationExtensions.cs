using System;
using System.Collections.Generic;
using Accounts.Domain.Events.Private;
using PublicEvents = Accounts.Domain.Events.Public;
using PrivateEvents = Accounts.Domain.Events.Private;
using System.Text;
using Accounts.Domain;

namespace EventPublisher
{
    public static class AccountEventsTranslationExtensions
    {
        internal static PublicEvents.AccountCreated ToPublic(this PrivateEvents.AccountCreated persistedEvent)
        {
            return new PublicEvents.AccountCreated (persistedEvent.AccountId, persistedEvent.Amount);
        }
        internal static PublicEvents.AccountCredited ToPublic(this PrivateEvents.AccountCredited persistedEvent)
        {
            return new PublicEvents.AccountCredited(persistedEvent.AccountId,persistedEvent.Amount);
        }
        internal static PublicEvents.AccountDebited ToPublic(this PrivateEvents.AccountDebited persistedEvent)
        {
            return new PublicEvents.AccountDebited(persistedEvent.AccountId, persistedEvent.Amount);
        }
    }
}
