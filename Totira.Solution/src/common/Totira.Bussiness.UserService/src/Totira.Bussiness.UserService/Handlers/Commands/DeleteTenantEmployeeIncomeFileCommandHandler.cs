using LanguageExt;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Totira.Bussiness.UserService.Commands;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.Events;
using Totira.Support.Application.Messages;
using Totira.Support.CommonLibrary.Interfaces;
using Totira.Support.TransactionalOutbox;
using static Totira.Support.Application.Messages.IMessageHandler;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Commands;

public class DeleteTenantEmployeeIncomeFileCommandHandler : IMessageHandler<DeleteTenantEmployeeIncomeFileCommand>
{
    private readonly IS3Handler _s3Handler;
    private readonly IRepository<TenantEmployeeIncomes, Guid> _repository;
    private readonly ILogger<DeleteTenantEmployeeIncomeFileCommandHandler> _logger;
    private readonly IContextFactory _contextFactory;
    private readonly IMessageService _messageService;

    public DeleteTenantEmployeeIncomeFileCommandHandler(
        IS3Handler s3Handler,
        IRepository<TenantEmployeeIncomes, Guid> repository,
        ILogger<DeleteTenantEmployeeIncomeFileCommandHandler> logger,
        IContextFactory contextFactory,
        IMessageService messageService
        )
    {
        _s3Handler = s3Handler;
        _repository = repository;
        _logger = logger;
        _messageService = messageService;
        _contextFactory = contextFactory;
    }

    public async Task HandleAsync(IContext context, Either<Exception, DeleteTenantEmployeeIncomeFileCommand> command)
    {
        await command.MatchAsync(async cmd => {
            var tenant = await _repository.GetByIdAsync(cmd.TenantId);
            _logger.LogInformation("Tenant ID: {TenantId}", cmd.TenantId);

            if (tenant is null || tenant.EmployeeIncomes is null)
                return;

            var income = tenant
                .EmployeeIncomes
                .FirstOrDefault(income => income.Id == cmd.IncomeId);

            if (income is null)
                return;

            tenant.EmployeeIncomes.Remove(income);

            var fileToDelete = income.Files.Where(f => f.FileName == cmd.FileName).FirstOrDefault();

            if (fileToDelete is null) return;

            income.Files.Remove(fileToDelete);
            tenant.EmployeeIncomes.Add(income);


            _logger.LogInformation("Removing file from the Db, file name: {FileName}", cmd.FileName);
            await _repository.Update(tenant);
            _logger.LogInformation("File removed from the Db, file name: {FileName}", cmd.FileName);
            _logger.LogInformation("Income ID: {IncomeId}", income.Id);


            var key = GetFormattedKeyName(
                        tenantId: cmd.TenantId.ToString(),
                        incomeId: cmd.IncomeId.ToString(),
                        fileName: cmd.FileName);

            var deleted = await _s3Handler.DeleteObjectAsync(key);

            if (deleted)
            {
                var fileDeletedEvent = new TenantEmployeeIncomeFileDeletedEvent(
                    cmd.TenantId,
                    cmd.IncomeId,
                    cmd.FileName);
                var notificationContext = _contextFactory.Create(string.Empty, context.CreatedBy);
                var messageOutboxId = await _messageService.SendAsync(notificationContext, fileDeletedEvent);
            }
        }, ex => throw ex);
    }

    private static string GetFormattedKeyName(string tenantId, string incomeId, string fileName)
        => $"{tenantId}/{incomeId}/{fileName}";
}
