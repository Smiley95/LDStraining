using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using Accounts.Domain;

namespace Accounts.Domain
{
    public class MessageCodes
    {
        const ulong Base = 33_000_000_000;

        public static readonly IImmutableDictionary<Type, ulong> TypeToCode = new Dictionary<Type, ulong>
        {
            { typeof(CreateAccount), Base + 1 },
            { typeof(CreditAccount), Base + 2 },
            { typeof(DebitAccount), Base + 3 },
            { typeof(AccountCreated), Base + 4 },
            { typeof(AccountCredited), Base + 5 },
            { typeof(AccountDebited), Base + 6 },

        }.ToImmutableDictionary();

        public static readonly IImmutableDictionary<ulong, Type> CodeToType = TypeToCode.ToImmutableDictionary(
            keySelector: kvp => kvp.Value,
            elementSelector: kvp => kvp.Key);
    }
}
