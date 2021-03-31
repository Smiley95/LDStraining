using System;
using System.Collections.Generic;
using System.Text;
using Account.DDD.Commands;
using Account.DDD.Events;
using Linedata.Foundation.Domain.EventSourcing;
using Linedata.Foundation.Domain.Messaging;

namespace Account.DDD
{
    public class AccountHandler 
        : IHandleCommand<CreateAccount>
            , IHandleCommand<BlockAccount>
            , IHandleCommand<DeposeCash>
            , IHandleCommand<DeposeCheque>
            , IHandleCommand<SetOverdraftLimit>
            , IHandleCommand<SetWireTransferLimit>
            , IHandleCommand<UnblockAccount>
            , IHandleCommand<WireTransfer>
            , IHandleCommand<WithdrawAmount>
    {
        readonly IRepository _repository;

        public AccountHandler(IRepository repository)
        {
            _repository = repository;
        }

        public CommandResponse Handle(CreateAccount command)
        {
            if (_repository.Exists<AccountAggregate.BankAccount>(command.AccountId))
                throw new InvalidOperationException("An account with this ID already exists");

            var account = new AccountAggregate.BankAccount(command.AccountId, command.HolderName, new AccountCreated(command.AccountId, command.HolderName));

            _repository.Save(account);
            return command.Succeed();
        }

        public CommandResponse Handle(BlockAccount command)
        {
            if (!_repository.TryGetById<AccountAggregate.BankAccount>(command.AccountId, out var account))
                throw new InvalidOperationException("No account with such ID");
            account.Block(new Blocked(command.AccountId));
            _repository.Save(account);

            return command.Succeed();
        }

        public CommandResponse Handle(WithdrawAmount command)
        {
            if (!_repository.TryGetById<AccountAggregate.BankAccount>(command.AccountId, out var account))
                throw new InvalidOperationException("No account with such ID");
            account.WithdrawCash(command.Funds,new WithdrawnAmount(command.AccountId,command.Funds));
            _repository.Save(account);

            return command.Succeed();
        }

        public CommandResponse Handle(WireTransfer command)
        {
            if (!_repository.TryGetById<AccountAggregate.BankAccount>(command.AccountId, out var account))
                throw new InvalidOperationException("No account with such ID");
            account.TransferWire(command.Funds,new WireTransferred(command.AccountId,command.Funds));
            _repository.Save(account);

            return command.Succeed();
        }

        public CommandResponse Handle(UnblockAccount command)
        {
            if (!_repository.TryGetById<AccountAggregate.BankAccount>(command.AccountId, out var account))
                throw new InvalidOperationException("No account with such ID");
            account.Unblock(new Unblocked(command.AccountId));
            _repository.Save(account);

            return command.Succeed();
        }
        public CommandResponse Handle(SetOverdraftLimit command)
        {
            if (!_repository.TryGetById<AccountAggregate.BankAccount>(command.AccountId, out var account))
                throw new InvalidOperationException("No account with such ID");
            account.SetOverdraftLimit(command.Limit,new SettedOverdraftLimit(command.AccountId,command.Limit));
            _repository.Save(account);

            return command.Succeed();
        }

        public CommandResponse Handle(SetWireTransferLimit command)
        {
            if (!_repository.TryGetById<AccountAggregate.BankAccount>(command.AccountId, out var account))
                throw new InvalidOperationException("No account with such ID");
            account.SetDailyWireTransferLimit(command.Limit,new SettedWireTransferLimit(command.AccountId,command.Limit));
            _repository.Save(account);

            return command.Succeed();
        }

        public CommandResponse Handle(DeposeCash command)
        {
            if (!_repository.TryGetById<AccountAggregate.BankAccount>(command.AccountId, out var account))
                throw new InvalidOperationException("No account with such ID");
            account.DeposeCash(command.Funds, new DeposedCash(command.AccountId, command.Funds));
            _repository.Save(account);

            return command.Succeed();
        }

        public CommandResponse Handle(DeposeCheque command)
        {
            //how to make it async and also implement the interface? 
            if (!_repository.TryGetById<AccountAggregate.BankAccount>(command.AccountId, out var account))
                throw new InvalidOperationException("No account with such ID");
            account.DeposeCheque(command.Funds,new DeposedCheque(command.AccountId,command.Funds)).Wait();
            _repository.Save(account);

            return command.Succeed();
        }
    }
}
