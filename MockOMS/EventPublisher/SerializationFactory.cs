using System;
using System.Collections.Generic;
using System.Text;
using Accounts.Domain;
using Accounts.Domain.Events;
using Linedata.Foundation.EventStorage;
using Linedata.Gateway.Common.Values;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NodaTime;
using NodaTime.Serialization.JsonNet;

namespace EventPublisher
{
    public static class SerializationFactory
    {
        static readonly JsonSerializerSettings Settings = new JsonSerializerSettings().ConfigureForEventPublisher();

        static SerializationFactory()
        {
            var jsonSerializer = new JsonSerializer();
            jsonSerializer.Converters.Add(new StringEnumConverter());
        }

        public static JsonSerializerSettings ConfigureForEventPublisher(this JsonSerializerSettings settings)
        {
            settings.Converters.Add(new PropertyConverter());
            return settings.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);
        }

        public static void RegisterMessageSerialization(this SerializationRegistry serializationRegistry)
        {
            serializationRegistry
                .RegisterDefaults<AccountDebited>()
                .RegisterDefaults<AccountCredited>()
                .RegisterDefaults<AccountCreated>();
        }

        static SerializationRegistry RegisterDefaults<T>(this SerializationRegistry registry)
        {
            return registry.RegisterJson<T>(1, Settings);
        }
    }
}
