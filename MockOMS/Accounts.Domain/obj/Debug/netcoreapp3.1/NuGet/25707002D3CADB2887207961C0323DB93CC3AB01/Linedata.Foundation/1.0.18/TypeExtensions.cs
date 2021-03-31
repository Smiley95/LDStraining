// <auto-generated />
// ReSharper disable CheckNamespace


namespace Accounts.Domain.Utils
{
    using System;
    using System.Text;
    using Accounts.Domain.Annotations;
    using CodeAnalysis = System.Diagnostics.CodeAnalysis;

    [CodeAnalysis.ExcludeFromCodeCoverage]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    static class TypeExtensions
    {
        public static string GetRealTypeName(this Type t)
        {
            if (t == null)
                return "null";

            if (!t.IsGenericType)
                return t.Name;

            var sb = new StringBuilder();
            sb.Append(t.Name.Substring(0, t.Name.IndexOf('`')));
            sb.Append('<');
            var genericArgs = t.GetGenericArguments();
            for (var i = 0; i < genericArgs.Length; ++i)
            {
                if (i > 0)
                    sb.Append(',');
                sb.Append(GetRealTypeName(genericArgs[i]));
            }

            sb.Append('>');
            return sb.ToString();
        }
    }
}