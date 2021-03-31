using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Google.Protobuf;
using Google.Protobuf.Reflection;
using Linedata.Foundation.Domain.EventSourcing;

namespace Accounts.Domain.Events.Public
{
    public class AccountDebited : IMessage<AccountDebited>
    {
        public Identity AccountId;
        public decimal Amount;

        public AccountDebited(Identity accountId, decimal amount)
        {
            AccountId = accountId;
            Amount = amount;
        }
        public AccountDebited(AccountDebited evt)
        {
            AccountId = evt.AccountId;
            Amount = evt.Amount;
        }
        public MessageDescriptor Descriptor => null;

        public int CalculateSize()
        {
            int size = 0;
            if (AccountId != null)
            {
                size += 1 + CodedOutputStream.ComputeStringSize(AccountId.ToString());
            }
            if (Amount != 0)
            {
                size += 1 + CodedOutputStream.ComputeDoubleSize((double)Amount);
            }
            return size;
        }

        public AccountDebited Clone()
        {
            return new AccountDebited(this);
        }

        public bool Equals([AllowNull] AccountDebited other)
        {
            if (ReferenceEquals(other, null))
            {
                return false;
            }
            if (ReferenceEquals(other, this))
            {
                return true;
            }

            return AccountId != other.AccountId;
        }

        public void MergeFrom(AccountDebited other)
        {
            if (other == null)
            {
                return;
            }

            Amount = other.Amount;
        }

        public void MergeFrom(CodedInputStream input)
        {
            input.ReadRawMessage(this);
        }

        public void WriteTo(CodedOutputStream output)
        {
            output.WriteRawMessage(this);
        }
    }
}
