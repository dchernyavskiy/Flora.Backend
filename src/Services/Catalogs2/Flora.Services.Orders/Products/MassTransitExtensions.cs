using Flora.Services.Orders.Products.Features.CreatingProduct.v1.Events.Integration.External;
using Flora.Services.Shared.Catalogs.Products.Events.v1.Integration;
using Humanizer;
using MassTransit;
using RabbitMQ.Client;

namespace Flora.Services.Orders.Products;

public static class MassTransitExtensions
{
    internal static void AddProductEndpoints(this IRabbitMqBusFactoryConfigurator cfg, IBusRegistrationContext context)
    {
        cfg.ReceiveEndpoint(
            nameof(ProductCreatedV1).Underscore().Prefixify(),
            re =>
            {
                re.ConfigureConsumeTopology = true;
                re.SetQuorumQueue();
                re.Bind(
                    $"{nameof(ProductCreatedV1).Underscore()}.input_exchange",
                    e =>
                    {
                        e.RoutingKey = nameof(ProductCreatedV1).Underscore();
                        e.ExchangeType = ExchangeType.Fanout;
                    });
                re.ConfigureConsumer<ProductCreatedConsumer>(context);

                re.RethrowFaultedMessages();
            });
    }
}
