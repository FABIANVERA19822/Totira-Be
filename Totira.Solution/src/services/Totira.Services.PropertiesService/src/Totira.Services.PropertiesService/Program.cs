using AutoMapper;
using CrestApps.RetsSdk.Models;
using CrestApps.RetsSdk.Models.Enums;
using Totira.Bussiness.PropertiesService.Context;
using Totira.Bussiness.PropertiesService.Extensions;
using Totira.Bussiness.PropertiesService.Mappers;
using Totira.Support.Api.Extensions;
using Totira.Support.Api.Options;
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
builder.Services.Configure<FrontendSettings>(builder.Configuration.GetSection("FrontendSettings"));
 
builder.Services.Configure<SesSettings>(builder.Configuration.GetSection("SesSettings"));
builder.Services.Configure<S3Settings>(builder.Configuration.GetSection("S3Settings"));


// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddHealthChecks();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//
builder.Services.AddQueryRestClient();

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

//automapper

var mapperConfig = new MapperConfiguration(cfg =>
{
    cfg.AddProfile(new PropertyProfile());
    cfg.AddProfile(new ResidentialProfile());
    cfg.AddProfile(new CondoProfile());
});

builder.Services.AddSingleton(mapperConfig.CreateMapper());

//Rest Client Config
builder.Services.Configure<RestClientOptions>(builder.Configuration.GetSection("RestClient")); 
builder.Services.AddConfigurableOptions(builder);


// Add Persistance
builder.Services.AddPersistance<MongoDBContext>();

//Event bus
var optionsQueue = Configuration.GetSection("EventBus:RabbitMQ").Get<RabbitMQOptions>() ?? new RabbitMQOptions();
builder.Services.AddEventBus(optionsQueue);

//RETS Options
builder.Services.AddOptions<ConnectionOptions>()
         .Configure((opts) =>
         {
             opts.UserAgent = builder.Configuration.GetSection("RETSSettings").GetValue<string>("UserAgent");
             opts.RetsServerVersion = RetsVersion.Make(builder.Configuration.GetSection("RETSSettings").GetValue<string>("ServerVersion"));
             opts.LoginUrl = builder.Configuration.GetSection("RETSSettings").GetValue<string>("LoginUrl");
             opts.Username = builder.Configuration.GetSection("RETSSettings").GetValue<string>("Username");
             opts.Password = builder.Configuration.GetSection("RETSSettings").GetValue<string>("Password");
             opts.UserAgentPassward = builder.Configuration.GetSection("RETSSettings").GetValue<string>("UserAgentPassward");
             opts.Type = Enum.Parse<AuthenticationType>("Digest", true);
             opts.Timeout = TimeSpan.FromHours(1);
         });




var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseAuthorization();

app.MapControllers();

await app.UseMLSEventBus();
app.MapHealthChecks("/healthz");

app.Run();
