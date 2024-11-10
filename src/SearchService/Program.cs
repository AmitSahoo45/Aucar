using System.Net;
using MassTransit;
using Polly;
using Polly.Extensions.Http;

using SearchService.Consumers;
using SearchService.Data;
using SearchService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddHttpClient<AuctionSvcHttpClient>().AddPolicyHandler(GetPolicy());
builder.Services.AddMassTransit(x =>
{
    x.AddConsumersFromNamespaceContaining<AuctionCreatedConsumer>();
    // Default formatting of consumer names is not a good practice.
    // because if in another service a consumer is created with the similar name
    // then some extra stuff needs to be added to the name so thats it different from the other service.
    // To achieve that we will be using an extra bit of configuration to set the endpoint. 

    x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("search", false)); // This false represents do we want to include the namespace with the formatted name. 

    x.UsingRabbitMq((context, cfg /*cfg is the configurations*/) =>
    {
        cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

app.UseAuthorization();
app.MapControllers();
app.Lifetime.ApplicationStarted.Register(async () =>
{
    try
    {
        await DbInitializer.InitDB(app);
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error initializing database" + ex.Message);
    }
});

app.Run();

static IAsyncPolicy<HttpResponseMessage> GetPolicy()
    => HttpPolicyExtensions
        .HandleTransientHttpError()
        .OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
        .WaitAndRetryForeverAsync(_ => TimeSpan.FromSeconds(10));