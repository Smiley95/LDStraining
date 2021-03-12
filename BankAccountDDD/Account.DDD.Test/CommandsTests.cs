using System;
using System.Collections.Generic;
using System.Text;
using Account.DDD.Commands;
using Account.DDD.Events;
using Xunit;

namespace Account.DDD.Test
{
    public class CommandsTests
    {
        [Fact]
        public void Can_create_createAccount_Command()
        {
            var id = Guid.NewGuid();
            var holderName = "someone";
            var @command = new CreateAccount(id, holderName);
            Assert.NotNull(@command);
            Assert.Equal(id, @command.AccountId);
            Assert.Equal(holderName, @command.HolderName);
        }

        [Fact]
        public void Can_create_BlockAccount_Command()
        {
            var id = Guid.NewGuid();
            var @command = new BlockAccount(id);
            Assert.NotNull(@command);
            Assert.Equal(id, @command.AccountId);
        }

        [Fact]
        public void Can_create_DeposeCash_Command()
        {
            var id = Guid.NewGuid();
            var funds = 250;
            var @command = new DeposeCash(id, funds);
            Assert.NotNull(@command);
            Assert.Equal(id, @command.AccountId);
            Assert.Equal(funds, @command.Funds);
        }

        [Fact]
        public void Can_create_DeposeCheque_Command()
        {
            var id = Guid.NewGuid();
            var funds = 250;
            var @command = new DeposeCheque(id, funds);
            Assert.NotNull(@command);
            Assert.Equal(id, @command.AccountId);
            Assert.Equal(funds, @command.Funds);
        }

        [Fact] 
        public void Can_create_SetOverdraftLimit_Command()
        {
            var id = Guid.NewGuid();
            var limit = 250;
            var @command = new SetOverdraftLimit(id, limit);
            Assert.NotNull(@command);
            Assert.Equal(id, @command.AccountId);
            Assert.Equal(limit, @command.Limit);
        }

        [Fact]
        public void Can_create_SetWireTransferLimit_Command()
        {
            var id = Guid.NewGuid();
            var limit = 250;
            var @command = new SetWireTransferLimit(id, limit);
            Assert.NotNull(@command);
            Assert.Equal(id, @command.AccountId);
            Assert.Equal(limit, @command.Limit);
        }

        [Fact]
        public void Can_create_UnblockedAccount_Command()
        {
            var id = Guid.NewGuid();
            var @command = new UnblockAccount(id);
            Assert.NotNull(@command);
            Assert.Equal(id, @command.AccountId);
        }

        [Fact]
        public void Can_create_WireTransfer_Command()
        {
            var id = Guid.NewGuid();
            var funds = 300;
            var @command = new WireTransfer(id,funds);
            Assert.NotNull(@command);
            Assert.Equal(id, @command.AccountId);
            Assert.Equal(funds, @command.Funds);
        }

        [Fact]
        public void Can_create_WithdrawAmount_Command()
        {
            var id = Guid.NewGuid();
            var funds = 300;
            var @command = new WithdrawAmount(id, funds);
            Assert.NotNull(@command);
            Assert.Equal(id, @command.AccountId);
            Assert.Equal(funds, @command.Funds);
        }

    }
}
