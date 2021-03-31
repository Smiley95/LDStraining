using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Accounts.Domain;
using EventPublisher.Configuration;
using Linedata.Foundation.Messaging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Linedata.Foundation.EventPublisher;
using Linedata.Foundation.EventStorage;
using Linedata.Foundation.EventStorage.EventStore;
using Linedata.Foundation.Messaging.Core;
using Linedata.Foundation.Messaging.Serialization;
using Linedata.Foundation.Messaging.Serialization.Protobuf;
using Linedata.Foundation.Messaging.Steeltoe;
using Linedata.Foundation.Messaging.Transport.ZeroMq;
using Linedata.Foundation.Secrets.Extensions;

namespace EventPublisher
{
    [ExcludeFromCodeCoverage]
    public static class ServiceCollectionExtensions
    {
        public static void AddGatewayEventPublisher(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMessaging();
            services.AddMessagingSteeltoe();
            services.AddZeroMqTransport();
            services.AddSerializers(msb => msb.AddEventPublisherSerializer());

            services.AddEventPublisherSettings(configuration);
            services.AddSecretStore(configuration);
            services.ConfigureEventStorage(configuration);
            services.AddEventStorageSerialization(registry => registry.RegisterMessageSerialization());
            services.AddEventPublisher().ConfigureForGateway();
        }

        static void AddEventPublisherSerializer(this MessageSerializerFactoryBuilder serializer)
        {
            serializer.AddProtobufSerializer(MessageCodes.TypeToCode);
        }

        static void ConfigureForGateway(this IEventPublisherBuilder eventPublisherBuilder)
        {
            eventPublisherBuilder
                .WithEventStorageProvider()
                .WithEventTranslator<EventToMessageTranslator>()
                .WithTopicGenerator<TopicGenerator>()
                .WithTopicStreamNameMapper<TopicStreamNameMapper>();
        }

        static void ConfigureEventStorage(this IServiceCollection services, IConfiguration configuration)
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
            Console.WriteLine("connection established "+ config.ToJson());
        }

        static void AddEventPublisherSettings(this IServiceCollection services, IConfiguration configuration)
        {
            var config = new ConfigRoot();
            configuration.Bind(config);

            var settings = new EventPublisherSettings
            {
                EndpointName = config.DuplexServerEndpoints[0].Name
            };
            Console.WriteLine("Duplex is here "+settings.ToJson());
            services.AddSingleton(settings);
        }
    }
}
