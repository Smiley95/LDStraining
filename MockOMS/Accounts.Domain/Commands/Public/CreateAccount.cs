using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using CommandReceiver.Commands;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Accounts.Domain.Commands.Public
{
    public class CreateAccount : IValidate, IMessage<CreateAccount>
    {
        public decimal Amount;

        public CreateAccount(decimal amount)
        {
            Amount = amount;
        }
        public CreateAccount(CreateAccount cmd)
        {
            Amount = cmd.Amount;
        }

        public MessageDescriptor Descriptor => null;

        public int CalculateSize()
        {
            int size = 0;
            if (Amount != 0)
            {
                size += 1 + CodedOutputStream.ComputeDoubleSize((double)Amount);
            }
            return size;
        }

        public CreateAccount Clone()
        {
            return new CreateAccount(this);
        }

        public bool Equals([AllowNull] CreateAccount other)
        {
            if (ReferenceEquals(other, null))
            {
                return false;
            }
            if (ReferenceEquals(other, this))
            {
                return true;
            }

            return Amount != other.Amount;
        }

        public void MergeFrom(CreateAccount other)
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

        public bool TryValidate(out string error)
        {
            var errorsText = new StringBuilder();

            if (Amount <= 0)
            {
                errorsText.Append($"{Amount}.");
            }

            error = errorsText.Length > 0 ? errorsText.ToString() : null;
            return error == null;
        }

        public void WriteTo(CodedOutputStream output)
        {
            output.WriteRawMessage(this);
        }
    }
}
