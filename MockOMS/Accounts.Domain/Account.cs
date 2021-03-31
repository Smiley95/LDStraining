using System;
using System.Runtime.CompilerServices;
using Accounts.Domain.Commands;
using Accounts.Domain.Events;
using PrivateEvents = Accounts.Domain.Events.Private;
using Accounts.Domain.Utils;
using Linedata.Foundation.Domain.EventSourcing;

namespace Accounts.Domain
{
    public class Account : Aggregate
    {
        public Identity AccountId;
        public decimal Balance;

        
        public Account(string ticker, int amount, decimal price)
        {
            if (string.IsNullOrEmpty(ticker) || amount <= 0 || price <= 0) throw new ArgumentException("wrong inputs!");
            Register<PrivateEvents.AccountCreated>(Apply);
            Register<PrivateEvents.AccountCredited>(Apply);
            Register<PrivateEvents.AccountDebited>(Apply);

            Raise(new PrivateEvents.AccountCreated(Identity.NewId(), amount));
        }

        public void Credit(int amount)
        {
            if (amount <= 0) throw new ArgumentException("wrong inputs!");
            Raise(new PrivateEvents.AccountCredited(AccountId, amount));
        }

        public void Debit(int amount)
        {
            if (amount <= 0 || amount > Balance) throw new ArgumentException("wrong inputs!");
            Raise(new PrivateEvents.AccountDebited(AccountId, amount));
        }
        
        private void Apply(PrivateEvents.AccountCreated @event)
        {
            AccountId= @event.AccountId;
            Balance = @event.Amount;
        }

        private void Apply(PrivateEvents.AccountCredited @event)
        {
            Balance += @event.Amount;
        }

        private void Apply(PrivateEvents.AccountDebited @event)
        {
            Balance -= @event.Amount;
        }
        
    }
}
