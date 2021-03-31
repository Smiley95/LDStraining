using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Linedata.Foundation.EventStorage;
using Linedata.Foundation.EventStorage.EventStore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CommandHandler
{
    [ExcludeFromCodeCoverage]
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureEventStorage(this IServiceCollection services, IConfiguration configuration)
        {
            var config = new ConfigRoot();
            configuration.Bind(config);
            services.AddSingleton(
                typeof(IStreamStore),
                sp => EventStoreBuilder
                    .WithConnectionString(config.EventStorage.ConnectionString)
                    .UseDefaultCredentials(
                        new UserCredentials(
                            username: config.EventStorage.UserName,
                            password: config.EventStorage.Password))
                    .DisableTlsConnection()
                    .KeepReconnecting()
                    .KeepRetrying()
                    .Build());
            Console.WriteLine("connection established " + config.ToJson());
        }
    }
}
