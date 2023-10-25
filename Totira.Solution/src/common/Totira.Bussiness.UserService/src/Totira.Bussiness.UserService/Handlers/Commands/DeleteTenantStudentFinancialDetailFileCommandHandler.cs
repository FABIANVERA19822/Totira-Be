using Microsoft.Extensions.Logging;
using Totira.Bussiness.UserService.Commands;
using Totira.Bussiness.UserService.Common;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.Domain.Common;
using Totira.Bussiness.UserService.Events;
using Totira.Support.Application.Messages;
using Totira.Support.CommonLibrary.Interfaces;
using Totira.Support.TransactionalOutbox;
using static Totira.Support.Application.Messages.IMessageHandler;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Commands;

public class DeleteTenantStudentFinancialDetailFileCommandHandler : IMessageHandler<DeleteTenantStudentFinancialDetailFileCommand>
{
    private readonly ILogger<DeleteTenantStudentFinancialDetailFileCommandHandler> _logger;
    private readonly IRepository<TenantEmployeeIncomes, Guid> _incomesRepository;
    private readonly ICommonFunctions _helper;
    private readonly IContextFactory _contextFactory;
    private readonly IMessageService _messageService;

    public DeleteTenantStudentFinancialDetailFileCommandHandler(
        ILogger<DeleteTenantStudentFinancialDetailFileCommandHandler> logger,
        IRepository<TenantEmployeeIncomes, Guid> studentFinancialDetailRepository,
        ICommonFunctions helper,
        IContextFactory contextFactory,
             IMessageService messageService)
    {
        _logger = logger;
        _incomesRepository = studentFinancialDetailRepository;
        _helper = helper;
        _contextFactory = contextFactory;
        _messageService = messageService;
    }

    public async Task HandleAsync(IContext context, DeleteTenantStudentFinancialDetailFileCommand command)
    {
        var tenant = await _incomesRepository.GetByIdAsync(command.TenantId);
        
        if (tenant.StudentIncomes is null)
        {
            _logger.LogError("Tenant not found: {tenantId}.", command.TenantId);
            return;
        }

        var studyDetail = tenant.StudentIncomes.FirstOrDefault(income => income.Id == command.StudyId);

        if (studyDetail is null)
        {
            _logger.LogError("Study detail not found: {studyId}.", command.StudyId);
            return;
        }

        Func<TenantFile, bool> isRequestedFile = (proof => proof.FileName == command.FileName);
        
        if (studyDetail.EnrollmentProofs.Any(isRequestedFile))
        {
            var file = studyDetail.EnrollmentProofs.First(isRequestedFile);
            var success = await _helper.DeleteFileAsync(file);
            if (success)
            {
                studyDetail.EnrollmentProofs.Remove(file);
                _logger.LogInformation("Proof of enrollment {fileName} deleted.", file.FileName);

                if (!studyDetail.EnrollmentProofs.Any())
                    _logger.LogInformation("Last proof of enrollment was deleted.");
            }
        }

        if (studyDetail.StudyPermitsOrVisas.Any(isRequestedFile))
        {
            var file = studyDetail.StudyPermitsOrVisas.First(isRequestedFile);
            var success = await _helper.DeleteFileAsync(file);
            if (success)
            {
                studyDetail.StudyPermitsOrVisas.Remove(file);
                _logger.LogInformation("Study permit or visa {fileName} deleted.", file.FileName);
                
                if (!studyDetail.StudyPermitsOrVisas.Any())
                    _logger.LogInformation("Last study permit or visa was deleted.");
            }
        }

        if (studyDetail.IncomeProofs.Any(isRequestedFile))
        {
            var file = studyDetail.IncomeProofs.First(isRequestedFile);
            var success = await _helper.DeleteFileAsync(file);
            if (success)
            {
                studyDetail.IncomeProofs.Remove(file);
                _logger.LogInformation("Proof of income {fileName} deleted.", file.FileName);

                if (!studyDetail.IncomeProofs.Any())
                    _logger.LogInformation("Last proof of income was deleted.");
            }

        }

        await _incomesRepository.Update(tenant);

        _logger.LogInformation("Student detail {studyId} updated.", studyDetail.Id);

        var studentFinancialDetailFileDeletedEvent = new TenantStudentFinancialDetailFileDeletedEvent(command.TenantId);
        var notificationContext = _contextFactory.Create(string.Empty, context.CreatedBy);
        var messageOutboxId = await _messageService.SendAsync(notificationContext, studentFinancialDetailFileDeletedEvent);
    }
}
