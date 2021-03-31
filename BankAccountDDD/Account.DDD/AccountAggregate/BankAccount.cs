using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Account.DDD.Events;
using Linedata.Foundation.Domain.EventSourcing;

namespace Account.DDD.AccountAggregate
{
    public class BankAccount : Aggregate
    {
        private Guid _accountId;
        private string _holderName;
        private decimal _overdraftLimit;
        private decimal _dailyWireTransferLimit;
        private bool _blocked;
        private decimal _amount;

        public BankAccount(Guid id, string holderName, AccountCreated @event) 
        {
            if (string.IsNullOrEmpty(holderName)) throw new ArgumentException("wrong inputs!");
            Register<AccountCreated>(Apply);
            Register<SettedOverdraftLimit>(Apply);
            Register<SettedWireTransferLimit>(Apply);
            Register<DeposedCash>(Apply);
            Register<WithdrawnAmount>(Apply);
            Register<WireTransferred>(Apply);
            Register<Blocked>(Apply);
            Register<Unblocked>(Apply);
            Register<DeposedCheque>(Apply);
            Raise(@event);
        }

        public BankAccount()
        {
            Register<AccountCreated>(Apply);
            Register<SettedOverdraftLimit>(Apply);
            Register<SettedWireTransferLimit>(Apply);
            Register<DeposedCash>(Apply);
            Register<WithdrawnAmount>(Apply);
            Register<WireTransferred>(Apply);
            Register<Blocked>(Apply);
            Register<Unblocked>(Apply);
            Register<DeposedCheque>(Apply);
        }


        public void SetOverdraftLimit(decimal limit, SettedOverdraftLimit @event)
        {
            if (limit <= 0) throw new ArgumentException("overdraftLimit should exceed 0");
            Raise(@event);
        }
        public void SetDailyWireTransferLimit(decimal limit, SettedWireTransferLimit @event)
        {
            if (limit <= 0) throw new ArgumentException("dailyWireTransferLimit should exceed 0");
            Raise(@event);
        }
        public async Task DeposeCheque(decimal checkAmount, DeposedCheque @event)
        {
            if (checkAmount <= 0) throw new ArgumentException("added amount should exceed 0");
            if (_blocked && (_amount + checkAmount <= 0)) throw new ArgumentException("not enough to unblock the account");
            if (DateTime.Today.DayOfWeek == DayOfWeek.Saturday || DateTime.Today.DayOfWeek == DayOfWeek.Sunday) throw new InvalidTimeZoneException("system out of work");
            if (DateTime.Today.DayOfWeek == DayOfWeek.Friday)
            {
                //await Task.Delay(DateTime.Today.AddDays(3).AddHours(9) - DateTime.Now); //for real time testing
                await Task.Delay(3000);
                Unblock(new Unblocked(_accountId));
                Raise(@event);
            }
            else
            {
                //await Task.Delay(DateTime.Today.AddDays(1).AddHours(9) - DateTime.Now); //for real time testing
                await Task.Delay(5000);
                Unblock(new Unblocked(_accountId));
                Raise(@event);
            }
        }
        public void DeposeCash(decimal cashFunds, DeposedCash @event)
        {
            if (cashFunds <= 0) throw new ArgumentException("invalid amount");
            if (_blocked && (_amount + cashFunds <= 0)) throw new ArgumentException("not enough to unblock the __account");
            if (_blocked) Unblock(new Unblocked(_accountId));
            Raise(@event);
        }
        public void WithdrawCash(decimal cashwithdrawn, WithdrawnAmount @event)
        {
            if (_blocked) throw new Exception("_account blocked");
            if (cashwithdrawn <= 0) throw new ArgumentException("Amount should exceed 0");
            if ((_amount + _overdraftLimit) - cashwithdrawn < 0)
            {
                Block(new Blocked(_accountId));
                Raise(@event);
                return;
            }

            Raise(@event);
        }
        public void TransferWire(decimal amountTranferred, WireTransferred @event)
        {
            if (_blocked) throw new Exception("_account blocked");
            if (amountTranferred <= 0) throw new ArgumentException("Amount should exceed 0");
            if ((_amount + _overdraftLimit) - amountTranferred < 0 || amountTranferred > _dailyWireTransferLimit)
            {
                Block(new Blocked(_accountId));
                return;
            }
            Raise(@event);
        }

        public void Block(Blocked @event) { Raise(@event); }
        public void Unblock(Unblocked @event) { Raise(@event); }

        private void Apply(AccountCreated @event)
        {
            Id = @event.AccountId;
            _accountId = @event.AccountId;
            _holderName = @event.HolderName;
            _blocked = false;
            _amount = 0;
            _dailyWireTransferLimit = 0;
            _overdraftLimit = 0;
        }
        private void Apply(Blocked @event)
        {
            _blocked = true;
        }

        private void Apply(Unblocked @event)
        {
            _blocked = false;
        }

        private void Apply(DeposedCheque @event)
        {
            _amount += @event.Funds;
        }

        private void Apply(DeposedCash @event)
        {
            _amount = _amount + @event.Funds;
        }

        private void Apply(SettedOverdraftLimit @event)
        {
            _overdraftLimit += @event.NewLimit;
        }
        private void Apply(SettedWireTransferLimit @event)
        {
            _dailyWireTransferLimit += @event.NewLimit;
        }
        private void Apply(WithdrawnAmount @event)
        {
            _amount -= @event.WithdrawnFunds;
        }

        private void Apply(WireTransferred @event)
        {
            _amount -= @event.TransferredFunds;
        }

    }
}
