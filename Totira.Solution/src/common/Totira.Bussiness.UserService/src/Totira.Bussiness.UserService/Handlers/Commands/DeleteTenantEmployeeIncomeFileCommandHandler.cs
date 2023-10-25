using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Totira.Bussiness.UserService.Commands;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.Events;
using Totira.Support.Application.Messages;
using Totira.Support.CommonLibrary.Interfaces;
using static Totira.Support.Application.Messages.IMessageHandler;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Commands;

public class DeleteTenantEmployeeIncomeFileCommandHandler : IMessageHandler<DeleteTenantEmployeeIncomeFileCommand>
{
    private readonly IS3Handler _s3Handler;
    private readonly IRepository<TenantEmployeeIncomes, Guid> _repository;
    private readonly ILogger<DeleteTenantEmployeeIncomeFileCommandHandler> _logger;

    public DeleteTenantEmployeeIncomeFileCommandHandler(
        IS3Handler s3Handler,
        IRepository<TenantEmployeeIncomes, Guid> repository,
        ILogger<DeleteTenantEmployeeIncomeFileCommandHandler> logger)
    {
        _s3Handler = s3Handler;
        _repository = repository;
        _logger = logger;
    }

    public async Task HandleAsync(IContext context, DeleteTenantEmployeeIncomeFileCommand command)
    {
        var tenant = await _repository.GetByIdAsync(command.TenantId);
        _logger.LogInformation("Tenant ID: {tenantId}", command.TenantId);

        if (tenant is null || tenant.EmployeeIncomes is null)
            return;

        var income = tenant
            .EmployeeIncomes
            .FirstOrDefault(income => income.Id == command.IncomeId);

        if (income is null)
            return;

        tenant.EmployeeIncomes.Remove(income);

        var fileToDelete = income.Files.Where(f => f.FileName == command.FileName).FirstOrDefault();

        if (fileToDelete is null) return;


        income.Files.Remove(fileToDelete);

        tenant.EmployeeIncomes.Add(income);


        _logger.LogInformation("Removing file from the Db", command.FileName);
        await _repository.Update(tenant);
        _logger.LogInformation("File removed from the Db", command.FileName);

        _logger.LogInformation("Income ID: {incomeId}", income.Id);


        var key = GetFormattedKeyName(
                    tenantId: command.TenantId.ToString(),
                    incomeId: command.IncomeId.ToString(),
                    fileName: command.FileName);

        var deleted = await _s3Handler.DeleteObjectAsync(key);

        if (deleted)
        {
            var fileDeletedEvent = new TenantEmployeeIncomeFileDeletedEvent(
                command.TenantId,
                command.IncomeId,
                command.FileName);
        }
    }

    private static string GetFormattedKeyName(string tenantId, string incomeId, string fileName)
        => $"{tenantId}/{incomeId}/{fileName}";
}
