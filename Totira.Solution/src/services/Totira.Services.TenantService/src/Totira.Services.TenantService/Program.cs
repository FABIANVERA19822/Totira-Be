using Microsoft.EntityFrameworkCore;
using Totira.Business.TenantService.Context;
using Totira.Business.TenantService.DTO;
using Totira.Business.TenantService.Queries;
using Totira.Support.Application.Queries;
using Totira.Support.Persistance.PostgreSQL;
using Totira.Support.Persistance.PostgreSQL.Extensions;

var builder = WebApplication.CreateBuilder(args);
var Configuration = builder.Configuration;


var configPostgres = Configuration.GetSection("PostgreSqlSettings").Get<PostgreSqlSettings>() ?? new PostgreSqlSettings();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddPersistance<PostgreSqlDBContext>(configPostgres);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/tenant/{id}", async (Guid id, IQueryHandler<QueryTenantById, GetTenantDto> _getTenantByIdHandler) =>
{
    return await _getTenantByIdHandler.HandleAsync(new QueryTenantById(id));
})
.WithName("GetTenantById")
.WithOpenApi();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PostgreSqlDBContext>();
    if (db.Database.GetPendingMigrations().Any())
    {
        db.Database.Migrate();
    }
}

app.Run();

