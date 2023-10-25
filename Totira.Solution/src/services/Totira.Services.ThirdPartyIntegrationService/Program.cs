using Totira.Business.Integration.Certn.Configurations;
using Totira.Business.Integration.Certn.Extensions;
using Totira.Business.ThirdPartyIntegrationService.Context;
using Totira.Business.ThirdPartyIntegrationService.Extensions;
using Totira.Business.ThirdPartyIntegrationService.Options;
using Totira.Support.Api.Extensions;
using Totira.Support.Api.Options;
using Totira.Support.CommonLibrary.Configurations;
using Totira.Support.EventServiceBus.RabittMQ;
using Totira.Support.EventServiceBus.RabittMQ.Extensions;
using Totira.Support.Persistance.Mongo;
using Totira.Support.Persistance.Mongo.Extensions;
using Totira.Support.Resilience.Polly.Extensions;
using Totira.Support.CommonLibrary.Extensions;
using Totira.Support.CommonLibrary.Settings;

var builder = WebApplication.CreateBuilder(args);
var Configuration = builder.Configuration;

//Settings
builder.Services.Configure<MongoSettings>(builder.Configuration.GetSection("MongoSettings"));
builder.Services.Configure<CertnSettings>(builder.Configuration.GetSection("CertnSettings"));
builder.Services.Configure<GoogleLocationServiceSettings>(builder.Configuration.GetSection("GoogleLocationServiceSettings"));
builder.Services.Configure<FrontendSettings>(builder.Configuration.GetSection("FrontendSettings"));


//Rest Client
builder.Services.AddQueryRestClient();
//Rest Client Config
builder.Services.Configure<RestClientOptions>(builder.Configuration.GetSection("RestClient"));
builder.Services.AddConfigurableOptions(builder);

//Jira Config
builder.Services.Configure<JiraOptions>(builder.Configuration.GetSection("Jira"));

//Persona Config
builder.Services.Configure<PersonaOptions>(builder.Configuration.GetSection("Persona"));

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddHealthChecks();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Retry polly
builder.Services.AddPolicyFactory();

//Api versioning
builder.Services.AddApiVersioning();

// Add Services
builder.Services.AddServices();
builder.Services.AddCertnLibrary();

// Add Common Library
builder.Services.AddCommonLibrary();



// Add Repositories
builder.Services.AddRepositories();


// Add Persistance
builder.Services.AddPersistance<MongoDBContext>();

//Event bus
var optionsQueue = Configuration.GetSection("EventBus:RabbitMQ").Get<RabbitMQOptions>() ?? new RabbitMQOptions();
builder.Services.AddEventBus(optionsQueue);

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

await app.UseThirdPartyIntegrationEventBus();
app.MapHealthChecks("/healthz");

app.Run();
