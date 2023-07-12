using Flora.Services.Shared.Customers.Customers.Events.v1.Integration;
using MassTransit;

namespace Flora.Services.Orders.Customers.Features.CreatingCustomer.v1.Events.External;

public class CustomerCreatedConsumer : IConsumer<CustomerCreatedV1>
{
    public Task Consume(ConsumeContext<CustomerCreatedV1> context)
    {
        return Task.CompletedTask;
    }
}
