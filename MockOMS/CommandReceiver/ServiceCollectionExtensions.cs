using Accounts.Domain.Commands.Public;
using Linedata.Foundation.Messaging;
using Linedata.Foundation.Messaging.Serialization;
using Microsoft.Extensions.DependencyInjection;

namespace CommandReceiver
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterSampleMessages(
            this IServiceCollection services,
            IMessageSerializer serializer)
        {
            return services.AddSerializers(
                msb =>
                    msb.AddSerializer(
                            typeof(CreateAccount),
                            1005,
                            serializer)
                        .AddSerializer(
                            typeof(CreditAccount),
                            1006,
                            serializer)
                        .AddSerializer(
                            typeof(DebitAccount),
                            1007,
                            serializer));
        }
    }
}
