using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Account.DDD.Events;
using Linedata.Foundation.Domain.EventSourcing;

namespace Account.DDD.AccountAggregate
{
    public class Account : Aggregate
    {
        public readonly Guid AccountId;
        public string HolderName;
        private double _overdraftLimit;
        public double OverdraftLimit
        {
            get { return _overdraftLimit; }
        }
        private double _dailyWireTransferLimit;
        public double DailyWireTransferLimit
        {
            get { return _dailyWireTransferLimit; }
        }
        private bool _blocked;
        public bool Blocked
        {
            get { return _blocked; }
        }
        public double Amount;

        public Account(Guid id, string holderName, AccountCreated @event) 
        {
            //Ensure.NotEmptyGuid(id, nameof(id));
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

        public void SetOverdraftLimit(double limit, SettedOverdraftLimit @event)
        {
            if (limit <= 0) throw new ArgumentException("overdraftLimit should exceed 0");
            Raise(@event);
        }
        public void SetDailyWireTransferLimit(double limit, SettedWireTransferLimit @event)
        {
            if (limit <= 0) throw new ArgumentException("dailyWireTransferLimit should exceed 0");
            Raise(@event);
        }
        public async Task DeposeCheque(double checkAmount, DeposedCheque @event)
        {
            if (checkAmount <= 0) throw new ArgumentException("added amount should exceed 0");
            if (_blocked && (Amount + checkAmount <= 0)) throw new ArgumentException("not enough to unblock the account");
            if (DateTime.Today.DayOfWeek == DayOfWeek.Saturday || DateTime.Today.DayOfWeek == DayOfWeek.Sunday) throw new InvalidTimeZoneException("system out of work");
            if (DateTime.Today.DayOfWeek == DayOfWeek.Friday)
            {
                //await Task.Delay(DateTime.Today.AddDays(3).AddHours(9) - DateTime.Now); //for real time testing
                await Task.Delay(3000);
                Unblock(new Unblocked(AccountId));
                Raise(@event);
            }
            else
            {
                //await Task.Delay(DateTime.Today.AddDays(1).AddHours(9) - DateTime.Now); //for real time testing
                await Task.Delay(5000);
                Unblock(new Unblocked(AccountId));
                Raise(@event);
            }
        }
        public void DeposeCash(double cashFunds, DeposedCash @event)
        {
            if (cashFunds <= 0) throw new ArgumentException("invalid amount");
            if (_blocked && (Amount + cashFunds <= 0)) throw new ArgumentException("not enough to unblock the account");
            if (_blocked) Unblock(new Unblocked(AccountId));
            Raise(@event);
        }
        public void WithdrawCash(double cashwithdrawn, WithdrawnAmount @event)
        {
            if (_blocked) throw new Exception("account blocked");
            if (cashwithdrawn <= 0) throw new ArgumentException("Amount should exceed 0");
            if ((Amount + _overdraftLimit) - cashwithdrawn < 0)
            {
                Block(new Blocked(AccountId));
                return;
            }

            Raise(@event);
        }
        public void TransferWire(double amountTranferred, WireTransferred @event)
        {
            if (_blocked) throw new Exception("account blocked");
            if (amountTranferred <= 0) throw new ArgumentException("Amount should exceed 0");
            if ((Amount + _overdraftLimit) - amountTranferred < 0 || amountTranferred > _dailyWireTransferLimit)
            {
                Block(new Blocked(AccountId));
                return;
            }
            Raise(@event);
        }

        public void Block(Blocked @event) { Raise(@event); }
        public void Unblock(Unblocked @event) { Raise(@event); }

        private void Apply(AccountCreated @event)
        {
            Id = @event.AccountId;
            HolderName = @event.HolderName;
            _blocked = false;
            Amount = 0;
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
            Amount += @event.Funds;
        }

        private void Apply(DeposedCash @event)
        {
            Amount = Amount + @event.Funds;
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
            Amount -= @event.WithdrawnFunds;
        }

        private void Apply(WireTransferred @event)
        {
            Amount -= @event.TransferredFunds;
        }

    }
}
