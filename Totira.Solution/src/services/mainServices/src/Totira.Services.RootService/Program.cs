using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Net;
using Totira.Services.RootService.Extensions;
using Totira.Services.RootService.Options;
using Totira.Support.Api.Extensions;
using Totira.Support.Api.Options;
using Totira.Support.Application.Extensions;
using Totira.Support.EventServiceBus.RabittMQ;
using Totira.Support.EventServiceBus.RabittMQ.Extensions;
using Totira.Support.NotificationHub.SignalR.Extensions;
using Totira.Support.Resilience.Polly.Extensions;


var builder = WebApplication.CreateBuilder(args);

var Configuration = builder.Configuration;

Configuration.AddEnvironmentVariables();

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHealthChecks();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(type => type.ToString());
});
//Rest Client
builder.Services.AddQueryRestClient();
builder.Services.AddConfigurableOptions(builder);

//Api versioning
builder.Services.AddApiVersioning();

//Retry polly
builder.Services.AddPolicyFactory();

//Context Message
builder.Services.AddContextFactory();

//Http Context Access
builder.Services.AddHttpContextAccessor();

//Event bus
var test = Configuration.GetSection("EventBus:RabbitMQ").Get<RabbitMQOptions>() ?? new RabbitMQOptions();
builder.Services.AddEventBus(test);




//Rest Client Config
builder.Services.Configure<RestClientOptions>(builder.Configuration.GetSection("RestClient"));

// Event Hub
builder.Services.AddSignalRNotificationHub();

builder.Services.AddNotifications();


var awsCognitoSettings = builder.Configuration.GetSection("CognitoPool").Get<CognitoOptions>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKeyResolver = (s, securityToken, identifier, parameters) =>
                        {
                            // get JsonWebKeySet from AWS
                            var json = new WebClient().DownloadString(parameters.ValidIssuer + "/.well-known/jwks.json");
                            // serialize the result
                            var keys = JsonConvert.DeserializeObject<JsonWebKeySet>(json).Keys;
                            // cast the result to be the type expected by IssuerSigningKeyResolver
                            return (IEnumerable<SecurityKey>)keys;
                        },

                        ValidIssuer = $@"https://cognito-idp.us-east-1.amazonaws.com/{awsCognitoSettings.ProjectId}",
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateLifetime = true,
                        ValidAudience = awsCognitoSettings.ClientId,
                        ValidateAudience = true
                    };
                });




var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();


//Set Allowed Origins

var alllowedOrigins = Configuration.GetSection("AllowedOrigins").Get<List<string>>() ?? new List<string>();

app.UseCors(builder =>
{
    builder.WithOrigins(alllowedOrigins.ToArray())
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials();
});

app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/healthz");
app.MapNotificationHub("/hub");
app.ConfigureEventBus();


app.Run();
