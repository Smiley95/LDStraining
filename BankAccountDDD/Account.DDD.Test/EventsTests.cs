using System;
using Account.DDD.Events;
using Xunit;

namespace Account.DDD.Test
{
    public class EventsTests
    {
        [Fact]
        public void Can_create_accountCreated_Event()
        {
            var id = Guid.NewGuid();
            var @event = new AccountCreated(id, "someone");
            Assert.NotNull(@event);
            Assert.Equal("someone",@event.HolderName);
            Assert.Equal(id, @event.AccountId);
        }

        [Fact]
        public void Can_create_blocked_Event()
        {
            var id = Guid.NewGuid();
            var @event = new Blocked(id);
            Assert.NotNull(@event);
            Assert.Equal(id, @event.BlockedAccount);
        }

        [Fact]
        public void Can_create_DeposedCash_Event()
        {
            var id = Guid.NewGuid();
            var funds = 25.000;
            var @event = new DeposedCash(id, funds);
            Assert.NotNull(@event);
            Assert.Equal(id, @event.AccountId);
            Assert.Equal(funds, @event.Funds);
        }

        [Fact]
        public void Can_create_DeposedCheque_Event()
        {
            var id = Guid.NewGuid();
            var funds = 25.000;
            var @event = new DeposedCheque(id, funds);
            Assert.NotNull(@event);
            Assert.Equal(id, @event.AccountId);
            Assert.Equal(funds, @event.Funds);
        }

        [Fact]
        public void Can_create_Unblocked_Event()
        {
            var id = Guid.NewGuid();
            var @event = new Unblocked(id);
            Assert.NotNull(@event);
            Assert.Equal(id, @event.UnblockedAccount);
        }

        [Fact]
        public void Can_create_WithdrawnAmount_Event()
        {
            var id = Guid.NewGuid();
            var funds = 25.000;
            var @event = new WithdrawnAmount(id, funds);
            Assert.NotNull(@event);
            Assert.Equal(id, @event.AccountId);
            Assert.Equal(funds, @event.WithdrawnFunds);
        }

        [Fact]
        public void Can_create_WireTransferred_Event()
        {
            var id = Guid.NewGuid();
            var amount = 250;
            var @event = new WireTransferred(id, amount);
            Assert.NotNull(@event);
            Assert.Equal(id, @event.AccountId);
            Assert.Equal(amount, @event.TransferredFunds);
        }

        [Fact]
        public void Can_create_SettedOverdraftLimit_Event()
        {
            var id = Guid.NewGuid();
            var limit = 250.000;
            var @event = new SettedOverdraftLimit(id, limit);
            Assert.NotNull(@event);
            Assert.Equal(id, @event.AccountId);
            Assert.Equal(limit, @event.NewLimit);
        }

        [Fact]
        public void Can_create_SettedWireTransferLimit_Event()
        {
            var id = Guid.NewGuid();
            var limit = 250.000;
            var @event = new SettedWireTransferLimit(id, limit);
            Assert.NotNull(@event);
            Assert.Equal(id, @event.AccountId);
            Assert.Equal(limit, @event.NewLimit);
        }
    }
}
