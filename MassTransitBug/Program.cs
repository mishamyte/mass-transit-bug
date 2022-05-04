using MassTransit;
using MassTransitBug;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host
    .ConfigureLogging((_, loggingBuilder) => loggingBuilder.ClearProviders())
    .UseSerilog((ctx, cfg) => cfg.ReadFrom.Configuration(ctx.Configuration));

var services = builder.Services;

services.AddMassTransit(
    configurator =>
    {
        configurator.SetKebabCaseEndpointNameFormatter();

        configurator.UsingInMemory();

        configurator.AddConsumers(typeof(Program).Assembly);
    });

var app = builder.Build();

app.MapGet(
    "/",
    async (IPublishEndpoint publishEndpoint, CancellationToken cancellationToken) =>
    {
        await publishEndpoint.Publish<Foo>(new
            {
                Bars = Enumerable.Range(0, 5).Select(_ => new Bar())
            },
            cancellationToken);
    });

app.Run();