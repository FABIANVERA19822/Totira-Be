using Totira.Bussiness.UserService.Context;
using Totira.Bussiness.UserService.Extensions;
using Totira.Services.UserService.Middleware;
using Totira.Support.Api.Options;
using Totira.Support.Api.Extensions;
using Totira.Support.CommonLibrary.Configurations;
using Totira.Support.CommonLibrary.Extensions;
using Totira.Support.CommonLibrary.Settings;
using Totira.Support.EventServiceBus.RabittMQ;
using Totira.Support.EventServiceBus.RabittMQ.Extensions;
using Totira.Support.Persistance.Mongo;
using Totira.Support.Persistance.Mongo.Extensions;
using Totira.Support.Resilience.Polly.Extensions;

var builder = WebApplication.CreateBuilder(args);
var Configuration = builder.Configuration;

//Settings
builder.Services.Configure<MongoSettings>(builder.Configuration.GetSection("MongoSettings"));
builder.Services.Configure<SesSettings>(builder.Configuration.GetSection("SesSettings"));
builder.Services.Configure<FrontendSettings>(builder.Configuration.GetSection("FrontendSettings"));
builder.Services.Configure<S3Settings>(builder.Configuration.GetSection("S3Settings"));

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



// Add Repositories
builder.Services.AddRepositories();

// Add Common Library
builder.Services.AddCommonLibrary();

//Rest Client
builder.Services.AddQueryRestClient();
builder.Services.AddConfigurableOptions(builder);
builder.Services.Configure<RestClientOptions>(builder.Configuration.GetSection("RestClient"));


// Add Persistance
builder.Services.AddPersistance<MongoDBContext>();

//Event bus
var optionsQueue = Configuration.GetSection("EventBus:RabbitMQ").Get<RabbitMQOptions>() ?? new RabbitMQOptions();
builder.Services.AddEventBus(optionsQueue);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.UseAuthorization();

app.MapControllers();

await app.UseEventBus();
app.MapHealthChecks("/healthz");

app.Run();
