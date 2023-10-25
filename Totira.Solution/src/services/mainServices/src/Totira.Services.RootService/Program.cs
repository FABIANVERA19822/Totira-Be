using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Totira.Services.RootService.Extensions;
using Totira.Services.RootService.Filters;
using Totira.Services.RootService.Middleware;
using Totira.Services.RootService.Options;
using Totira.Support.Api.Extensions;
using Totira.Support.Api.Options;
using Totira.Support.Application.Extensions;
using Totira.Support.CommonLibrary.Configurations;
using Totira.Support.CommonLibrary.Extensions;
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

//Patterns Options for settings
builder.Services.Configure<RestClientOptions>(builder.Configuration.GetSection("RestClient"));
builder.Services.Configure<CognitoOptions>(builder.Configuration.GetSection("CognitoPool"));

var apiSecurityOptions = builder.Configuration.GetSection("ApiSecurityOptions").Get<ApiSecurityOptions>();
builder.Services.AddSingleton(apiSecurityOptions);

// Add custom filters
builder.Services.AddScoped<ApiKeyAuthorizationFilter>();
builder.Services.AddScoped<SafeIPsListFilter>();

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(type => type.ToString());
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.ApiKey,
        In = ParameterLocation.Header,
        Scheme = "bearer",
        Description = "Please insert JWT token into field",
        Name = "Authorization"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference =  new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
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

// Event Hub
builder.Services.AddSignalRNotificationHub();

builder.Services.AddNotifications();
builder.Services.AddCommonLibrary();

var awsCognitoSettings = builder.Configuration.GetSection("CognitoPool").Get<CognitoOptions>();

builder.Services.AddCognitoIdentity();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.Authority = $@"https://cognito-idp.us-east-1.amazonaws.com/{awsCognitoSettings.ProjectId}";
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateLifetime = true,
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("PublicOptions", policy => policy.RequireClaim("scope", "clientapp/client"));
    options.AddPolicy("AppOptions", policy => policy.RequireClaim("scope", "aws.cognito.signin.user.admin"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<AuthorizationMiddleware>();

//app.Use(async (context, next) =>
//{
//    context.Response.Headers.Remove("X-Powered-By");
//    context.Response.Headers.Add("X-Frame-Options", "DENY");
//    await next();
//});

//Set Allowed Origins

app.UseForwardedHeaders();

var alllowedOrigins = Configuration.GetSection("AllowedOrigins").Get<List<string>>() ?? new List<string>();

app.UseCors(builder =>
{
    builder.WithOrigins(alllowedOrigins.ToArray())
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials();
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/healthz");
app.MapNotificationHub("/hub");
app.ConfigureEventBus();

app.Run();