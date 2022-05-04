using MassTransit;

namespace MassTransitBug;

public class FooConsumer : IConsumer<Foo>
{
    public Task Consume(ConsumeContext<Foo> context)
    {
        return Task.CompletedTask;
    }
}