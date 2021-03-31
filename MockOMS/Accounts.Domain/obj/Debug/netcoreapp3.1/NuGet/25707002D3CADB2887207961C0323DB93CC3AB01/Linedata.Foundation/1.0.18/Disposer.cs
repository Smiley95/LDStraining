// <auto-generated />
// ReSharper disable CheckNamespace


namespace Accounts.Domain.Utils
{
    using System;
    using CodeAnalysis = System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Accounts.Domain.Annotations;

    [CodeAnalysis.ExcludeFromCodeCoverage]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    sealed class Disposer : IDisposable
    {
        [NotNull]
        public static IDisposable CreateEmpty() => new Disposer(() => { });

        readonly Action _disposer;
        bool _disposed;

        public Disposer([NotNull] Action disposer)
        {
            Ensure.IsNotNull(disposer, nameof(disposer));

            _disposer = disposer;
        }

        public Disposer([NotNull] Func<Task> disposer)
        {
            Ensure.IsNotNull(disposer, nameof(disposer));

            _disposer = () =>
            {
                disposer()?.GetAwaiter().GetResult();
            };
        }

        public void Dispose()
        {
            if (_disposed)
                return;

            try
            {
                _disposer?.Invoke();
            }
            catch
            {
                //ignore
            }

            _disposed = true;
        }
    }
}
