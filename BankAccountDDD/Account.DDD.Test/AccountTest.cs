using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using TestAccount = Account.DDD.AccountAggregate.Account;
using Account.DDD.Events;
using Xunit;

namespace Account.DDD.Test
{
    public class AccountTest
    {
        [Trait("Category","Initiation")]
        [Fact]
        public void Can_create_an_account()
        {
            var id = Guid.NewGuid();
            var holder = "someone";
            Assert.NotNull(new TestAccount(id, holder, new AccountCreated(id,holder)));
        }

        [Trait("Category", "Initiation")]
        [Fact]
        public void Cannot_create_an_account_with_invalid_params()
        {
            var id = Guid.NewGuid();
            Assert.Throws<ArgumentException>(() => new TestAccount(id, "", new AccountCreated(id, "")));
            Assert.Throws<ArgumentException>(() => new TestAccount(id, null, new AccountCreated(id, "  ")));
        }

        [Trait("Category", "BlockStatus")]
        [Fact]
        public void Can_block_Account()
        {
            var id = Guid.NewGuid();
            var account = new TestAccount(id, "holder", new AccountCreated(id, "holder"));
            Assert.False(account.Blocked);
            account.Block(new Blocked(account.AccountId));
            Assert.True(account.Blocked);
        }

        [Trait("Category", "BlockStatus")]
        [Fact]
        public void Can_unblock_Account()
        {
            var id = Guid.NewGuid();
            var account = new TestAccount(id, "holder", new AccountCreated(id, "holder"));
            account.Block(new Blocked(account.AccountId));
            Assert.True(account.Blocked);
            account.Unblock(new Unblocked(account.AccountId));
            Assert.False(account.Blocked);
        }

        [Trait("Category", "BlockStatus")]
        [Fact]
        public void Can_block_when_withdraw()
        {
            var id = Guid.NewGuid();
            var account = new TestAccount(id, "holder", new AccountCreated(id, "holder"));
            account.Amount = 250;
            account.WithdrawCash(300, new WithdrawnAmount(id, 300));
            Assert.True(account.Blocked);
        }

        [Trait("Category", "BlockStatus")]
        [Fact]
        public void Can_block_through_wire_transferring()
        {
            var id = Guid.NewGuid();
            var account = new TestAccount(id, "holder", new AccountCreated(id, "holder"));
            account.Amount = 250;
            account.SetDailyWireTransferLimit(100, new SettedWireTransferLimit(id,100));
            account.TransferWire(300, new WireTransferred(id, 300));
            Assert.True(account.Blocked);
        }

        [Trait("Category", "BlockStatus")]
        [Fact]
        public void Can_unblock_when_depose_cash()
        {
            var id = Guid.NewGuid();
            var account = new TestAccount(id, "holder", new AccountCreated(id, "holder"));
            account.Amount = 250;
            account.WithdrawCash(300, new WithdrawnAmount(id, 300));
            account.DeposeCash(150, new DeposedCash(id,150));
            Assert.Equal(400, account.Amount);
            Assert.False(account.Blocked);
        }

        [Trait("Category", "BlockStatus")]
        [Fact]
        public async Task Can_unblock_when_depose_cheque()
        {
            var id = Guid.NewGuid();
            var account = new TestAccount(id, "holder", new AccountCreated(id, "holder"));
            account.Amount = 250;
            account.WithdrawCash(300, new WithdrawnAmount(id, 300));
            await account.DeposeCheque(150, new DeposedCheque(id, 150));
            Assert.Equal(400, account.Amount);
            Assert.False(account.Blocked);
        }

        [Trait("Category", "LimitsUpdating")]
        [Fact]
        public void Can_set_overdraft_limit()
        {
            var id = Guid.NewGuid();
            var account = new TestAccount(id, "holder", new AccountCreated(id, "holder"));
            Assert.Equal(0,account.OverdraftLimit);
            account.SetOverdraftLimit(250,new SettedOverdraftLimit(id,250));
            Assert.Equal(250, account.OverdraftLimit);
        }

        [Trait("Category", "LimitsUpdating")]
        [Fact]
        public void Can_set_daily_wire_transfer_limit()
        {
            var id = Guid.NewGuid();
            var account = new TestAccount(id, "holder", new AccountCreated(id, "holder"));
            Assert.Equal(0, account.DailyWireTransferLimit);
            account.SetDailyWireTransferLimit(250, new SettedWireTransferLimit(id, 250));
            Assert.Equal(250, account.DailyWireTransferLimit);
        }

        [Trait("Category", "LimitsUpdating")]
        [Fact]
        public void Cannot_set_invalid_limit()
        {
            var id = Guid.NewGuid();
            var account = new TestAccount(id, "holder", new AccountCreated(id, "holder"));
            Assert.Throws<ArgumentException>(() => account.SetDailyWireTransferLimit(-120, new SettedWireTransferLimit(id,-120)));
            Assert.Throws<ArgumentException>(() => account.SetOverdraftLimit(-450, new SettedOverdraftLimit(id,-450)));
        }

        [Trait("Category", "Deposing")]
        [Fact]
        public void Can_depose_cash()
        {
            var id = Guid.NewGuid();
            var account = new TestAccount(id, "holder", new AccountCreated(id, "holder"));
            Assert.Equal(0, account.Amount);
            account.DeposeCash(400, new DeposedCash(id,400));
            Assert.Equal(400, account.Amount);
        }

        [Trait("Category", "Deposing")]
        [Fact]
        public async Task Can_depose_cheque()
        {
            var id = Guid.NewGuid();
            var account = new TestAccount(id, "holder", new AccountCreated(id, "holder"));
            Assert.Equal(0, account.Amount);
            await account.DeposeCheque(400, new DeposedCheque(id, 400));
            Assert.Equal(400, account.Amount);
        }

        [Trait("Category", "Deposing")]
        [Fact]
        public void Cannot_depose_invalid_amount_of_cash()
        {
            var id = Guid.NewGuid();
            var account = new TestAccount(id, "holder", new AccountCreated(id, "holder"));
            Assert.Equal(0, account.Amount);
            Assert.Throws<ArgumentException>(() => account.DeposeCash(0, new DeposedCash(id, 0)));
            Assert.Throws<ArgumentException>(() => account.DeposeCash(-120, new DeposedCash(id, -120)));
        }

        [Trait("Category", "Deposing")]
        [Fact]
        public void Cannot_depose_while_account_is_blocked()
        {
            var id = Guid.NewGuid();
            var account = new TestAccount(id, "holder", new AccountCreated(id, "holder"));
            account.Amount = -200;
            account.Block(new Blocked(id));
            Assert.Throws<ArgumentException>(() => account.DeposeCash(100, new DeposedCash(id, 100)));
        }

        [Trait("Category", "Deposing")]
        [Fact]
        public async Task Cannot_depose_invalid_cheque_amount()
        {
            var id = Guid.NewGuid();
            var account = new TestAccount(id, "holder", new AccountCreated(id, "holder"));
            await Assert.ThrowsAsync<ArgumentException>(() => account.DeposeCheque(0, new DeposedCheque(id, 0)));
            await Assert.ThrowsAsync<ArgumentException>(() => account.DeposeCheque(-120, new DeposedCheque(id, -120)));
        }

        [Trait("Category", "Deposing")]
        [Fact]
        public async Task Cannot_depose_cheque_to_blocked_account()
        {
            var id = Guid.NewGuid();
            var account = new TestAccount(id, "holder", new AccountCreated(id, "holder"));
            account.Amount = -250;
            account.Block(new Blocked(id));
            Assert.True(account.Blocked);
            await Assert.ThrowsAsync<ArgumentException>(() => account.DeposeCheque(100, new DeposedCheque(id, 100)));
        }

        [Trait("Category", "Withdraw")]
        [Fact]
        public void Can_withdraw_amount()
        {
            var id = Guid.NewGuid();
            var account = new TestAccount(id, "holder", new AccountCreated(id, "holder"));
            account.Amount = 250;
            account.WithdrawCash(150,new WithdrawnAmount(id,150));
            Assert.Equal(100, account.Amount);
        }

        [Trait("Category", "Withdraw")]
        [Fact]
        public void Cannot_withdraw_invalid_amount()
        {
            var id = Guid.NewGuid();
            var account = new TestAccount(id, "holder", new AccountCreated(id, "holder"));
            account.Amount = 250;
            Assert.Throws<ArgumentException>(() => account.WithdrawCash(0, new WithdrawnAmount(id, 0)));
            Assert.Throws<ArgumentException>(() => account.WithdrawCash(-120, new WithdrawnAmount(id, -120)));
        }

        [Trait("Category", "Withdraw")]
        [Fact]
        public void Cannot_withdraw_blocked_amount()
        {
            var id = Guid.NewGuid();
            var account = new TestAccount(id, "holder", new AccountCreated(id, "holder"));
            account.Amount = 250;
            account.WithdrawCash(300,new WithdrawnAmount(id,300));
            Assert.Throws<Exception>(() => account.WithdrawCash(20, new WithdrawnAmount(id, 20)));
        }

        [Trait("Category", "Wire transfer")]
        [Fact]
        public void Can_withdraw_through_wire_transferring()
        {
            var id = Guid.NewGuid();
            var account = new TestAccount(id, "holder", new AccountCreated(id, "holder"));
            account.Amount = 250;
            account.SetDailyWireTransferLimit(300, new SettedWireTransferLimit(id,300));
            account.TransferWire(200, new WireTransferred(id, 200));
            Assert.Equal(50, account.Amount);
        }

        [Trait("Category", "Wire transfer")]
        [Fact]
        public void Cannot_withdraw_invalid_wire_transferring_values()
        {
            var id = Guid.NewGuid();
            var account = new TestAccount(id, "holder", new AccountCreated(id, "holder"));
            account.Amount = 250;
            account.SetDailyWireTransferLimit(200, new SettedWireTransferLimit(id, 200));
            Assert.Throws<ArgumentException>(() => account.TransferWire(-250, new WireTransferred(id, -250)));
        }

        [Trait("Category", "Wire transfer")] 
        [Fact]
        public void Cannot_wire_transfer_from_blocked_account()
        {
            var id = Guid.NewGuid();
            var account = new TestAccount(id, "holder", new AccountCreated(id, "holder"));
            account.SetDailyWireTransferLimit(100, new SettedWireTransferLimit(id,100));
            account.Block(new Blocked(id));
            Assert.Throws<Exception>(() => account.TransferWire(100, new WireTransferred(id, 100)));
        }

    }
}
