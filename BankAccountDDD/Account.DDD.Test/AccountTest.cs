using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using Account.DDD.Commands;
using TestAccount = Account.DDD.AccountAggregate.BankAccount;
using Account.DDD.Events;
using Linedata.Foundation.Domain.EventSourcing;
using Linedata.Foundation.Domain.Messaging;
using Linedata.Foundation.EventStorage;
using Linedata.Foundation.EventStorage.InMemory;
using Newtonsoft.Json;
using Xunit;

namespace Account.DDD.Test
{
    public class AccountTest
    {

        private InMemoryEventStore _mockStore;
        private IStreamStoreConnection _mockStoreConnection;
        private StreamStoreRepository _repo;
        private AccountHandler _service;

        public AccountTest()
        {
            _mockStore = new InMemoryEventStore();
            _mockStoreConnection = _mockStore.Connect("Accounts");
            var eventSerializer = new JsonEventSerializer();
            _repo = new StreamStoreRepository(new PrefixedCamelCaseStreamNameBuilder(), _mockStoreConnection, eventSerializer);
            _service = new AccountHandler(_repo);

        }
        [Trait("Category","Initiation")]
        [Fact]
        public void Can_create_an_account()
        {
            var id = Guid.NewGuid();
            var holder = "someone";
            var command = new CreateAccount(id, holder);
            var reply = _service.Handle(command);
            Assert.Equal(typeof(Success), reply.GetType());
            Assert.True(_repo.TryGetById(command.AccountId,out TestAccount account));
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
            var createCommand = new CreateAccount(id, "someone");
            var reply = _service.Handle(createCommand);
            var account = _repo.GetById<TestAccount>(id);
            var blockCommand = new BlockAccount(id);
            Assert.Equal(typeof(Success), _service.Handle(blockCommand).GetType());
        }

        [Trait("Category", "BlockStatus")]
        [Fact]
        public void Can_unblock_Account()
        {
            var id = Guid.NewGuid();
            var createCommand = new CreateAccount(id, "someone");
            var reply = _service.Handle(createCommand);
            var unblockCommand = new UnblockAccount(id);
            Assert.Equal(typeof(Success), _service.Handle(unblockCommand).GetType());
        }

        [Trait("Category", "BlockStatus")]
        [Fact]
        public void Can_block_when_withdraw()
        {
            var id = Guid.NewGuid();
            var reply = _service.Handle(new CreateAccount(id, "someone"));
            reply = _service.Handle(new DeposeCash(id,250));
            Assert.Equal(typeof(Success), _service.Handle(new WithdrawAmount(id,300)).GetType());
        }

        [Trait("Category", "BlockStatus")]
        [Fact]
        public void Can_block_through_wire_transferring()
        {
            var id = Guid.NewGuid();
            var reply = _service.Handle(new CreateAccount(id, "someone"));
            reply = _service.Handle(new DeposeCash(id, 250));
            reply = _service.Handle(new SetWireTransferLimit(id, 100));
            Assert.Equal(typeof(Success), _service.Handle(new WireTransfer(id, 300)).GetType());
        }

        [Trait("Category", "BlockStatus")]
        [Fact]
        public void Can_unblock_when_depose_cash()
        {
            var id = Guid.NewGuid();
            var reply = _service.Handle(new CreateAccount(id, "someone"));
            reply = _service.Handle(new DeposeCash(id, 250));
            reply = _service.Handle(new BlockAccount(id));
            Assert.Equal(typeof(Success), _service.Handle(new DeposeCash(id, 150)).GetType());
        }

        [Trait("Category", "BlockStatus")]
        [Fact]
        public async Task Can_unblock_when_depose_cheque()
        {
            var id = Guid.NewGuid();
            var reply = _service.Handle(new CreateAccount(id, "someone"));
            reply = _service.Handle(new DeposeCash(id, 250));
            reply = _service.Handle(new BlockAccount(id));
            Assert.Equal(typeof(Success), _service.Handle(new DeposeCheque(id, 150)).GetType());
        }

        [Trait("Category", "LimitsUpdating")]
        [Fact]
        public void Can_set_overdraft_limit()
        {
            var id = Guid.NewGuid();
            var reply = _service.Handle(new CreateAccount(id, "someone"));
            Assert.Equal(typeof(Success), _service.Handle(new SetOverdraftLimit(id, 100)).GetType());// can add other check through the block and withdraw 
        }

        [Trait("Category", "LimitsUpdating")]
        [Fact]
        public void Can_set_daily_wire_transfer_limit()
        {
            var id = Guid.NewGuid();
            var reply = _service.Handle(new CreateAccount(id, "someone"));
            reply = _service.Handle(new DeposeCash(id, 250));
            Assert.Equal(typeof(Success), _service.Handle(new SetWireTransferLimit(id, 100)).GetType());// can add other check through the block and withdraw 
            
        }

        [Trait("Category", "LimitsUpdating")]
        [Fact]
        public void Cannot_set_invalid_limit()
        {
            var id = Guid.NewGuid();
            var reply = _service.Handle(new CreateAccount(id, "someone"));
            Assert.Throws<ArgumentException>(() => _service.Handle(new SetWireTransferLimit(id, -100)));
            Assert.Throws<ArgumentException>(() => _service.Handle(new SetOverdraftLimit(id, -120)));
        }

        [Trait("Category", "Deposing")]
        [Fact]
        public void Can_depose_cash()
        {
            var id = Guid.NewGuid();
            var reply = _service.Handle(new CreateAccount(id, "someone"));
            Assert.Equal(typeof(Success), _service.Handle(new DeposeCash(id, 100)).GetType());
        }

        [Trait("Category", "Deposing")]
        [Fact]
        public async Task Can_depose_cheque()
        {
            var id = Guid.NewGuid();
            var reply = _service.Handle(new CreateAccount(id, "someone"));
            Assert.Equal(typeof(Success), _service.Handle(new DeposeCheque(id, 100)).GetType());
        }

        [Trait("Category", "Deposing")]
        [Fact]
        public void Cannot_depose_invalid_amount_of_cash()
        {
            var id = Guid.NewGuid();
            var reply = _service.Handle(new CreateAccount(id, "someone"));
            Assert.Equal(typeof(Success), _service.Handle(new DeposeCheque(id, 100)).GetType());
            Assert.Throws<ArgumentException>(() => _service.Handle(new DeposeCash(id, 0)));
            Assert.Throws<ArgumentException>(() => _service.Handle(new DeposeCash(id, -120)));
        }

        [Trait("Category", "Deposing")]
        [Fact]
        public void Cannot_depose_while_account_is_blocked()
        {
            var id = Guid.NewGuid();
            var reply = _service.Handle(new CreateAccount(id, "someone"));
            Assert.Equal(typeof(Success), _service.Handle(new DeposeCash(id, 100)).GetType());
            Assert.Equal(typeof(Success), _service.Handle(new WithdrawAmount(id, 200)).GetType());
            Assert.Equal(typeof(Success), _service.Handle(new BlockAccount(id)).GetType());
            Assert.Throws<ArgumentException>(() => _service.Handle(new DeposeCash(id, 50)));
        }

        [Trait("Category", "Deposing")]
        [Fact]
        public async Task Cannot_depose_invalid_cheque_amount()
        {
            var id = Guid.NewGuid();
            var reply = _service.Handle(new CreateAccount(id, "someone"));
            Assert.Throws<AggregateException>( () => _service.Handle(new DeposeCheque(id, 0)));
            Assert.Throws<AggregateException>(() => _service.Handle(new DeposeCheque(id, -120)));
        }

        [Trait("Category", "Deposing")]
        [Fact]
        public async Task Cannot_depose_cheque_to_blocked_account()
        {
            var id = Guid.NewGuid();
            var reply = _service.Handle(new CreateAccount(id, "someone"));
            reply = _service.Handle(new DeposeCash(id, 200));
            reply = _service.Handle(new SetOverdraftLimit(id, 200));
            reply = _service.Handle(new WithdrawAmount(id, 450));
            Assert.Throws<AggregateException>(() => _service.Handle(new DeposeCheque(id, 100)));
        }

        [Trait("Category", "Withdraw")]
        [Fact]
        public void Can_withdraw_amount()
        {
            var id = Guid.NewGuid();
            var reply = _service.Handle(new CreateAccount(id, "someone"));
            Assert.Equal(typeof(Success), _service.Handle(new DeposeCash(id,100)).GetType());
            Assert.Equal(typeof(Success), _service.Handle(new WithdrawAmount(id,50)).GetType());
        }

        [Trait("Category", "Withdraw")]
        [Fact]
        public void Cannot_withdraw_invalid_amount()
        {
            var id = Guid.NewGuid();
            var reply = _service.Handle(new CreateAccount(id, "someone"));
            Assert.Equal(typeof(Success), _service.Handle(new DeposeCash(id, 200)).GetType());
            Assert.Throws<ArgumentException>(() => _service.Handle(new WithdrawAmount(id, 0)));
            Assert.Throws<ArgumentException>(() => _service.Handle(new WithdrawAmount(id, -120)));
        }

        [Trait("Category", "Withdraw")]
        [Fact]
        public void Cannot_withdraw_blocked_amount()
        {
            var id = Guid.NewGuid();
            var reply = _service.Handle(new CreateAccount(id, "someone"));
            reply = _service.Handle(new DeposeCash(id, 200));
            reply = _service.Handle(new WithdrawAmount(id, 300));
            Assert.Throws<Exception>(() => _service.Handle(new WithdrawAmount(id, 20)));
        }

        [Trait("Category", "Wire transfer")]
        [Fact]
        public void Can_withdraw_through_wire_transferring()
        {
            var id = Guid.NewGuid();
            var reply = _service.Handle(new CreateAccount(id, "someone"));
            reply = _service.Handle(new DeposeCash(id, 250));
            reply = _service.Handle(new SetWireTransferLimit(id, 300));
            Assert.Equal(typeof(Success), _service.Handle(new WireTransfer(id, 200)).GetType());
        }

        [Trait("Category", "Wire transfer")]
        [Fact]
        public void Cannot_withdraw_invalid_wire_transferring_values()
        {
            var id = Guid.NewGuid();
            var reply = _service.Handle(new CreateAccount(id, "someone"));
            reply = _service.Handle(new DeposeCash(id, 250));
            reply = _service.Handle(new SetWireTransferLimit(id, 200));
            Assert.Throws<ArgumentException>(() => _service.Handle(new WireTransfer(id, -200)));
        }

        [Trait("Category", "Wire transfer")]
        [Fact]
        public void Cannot_wire_transfer_from_blocked_account()
        {
            var id = Guid.NewGuid();
            var reply = _service.Handle(new CreateAccount(id, "someone"));
            reply = _service.Handle(new SetWireTransferLimit(id, 100));
            reply = _service.Handle(new BlockAccount(id));
            Assert.Throws<Exception>(() => _service.Handle(new WireTransfer(id,100)));
        }

    }
}
