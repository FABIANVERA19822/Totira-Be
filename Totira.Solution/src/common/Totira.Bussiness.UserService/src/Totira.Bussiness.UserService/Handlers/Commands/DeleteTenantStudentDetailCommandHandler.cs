using LanguageExt;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Totira.Bussiness.UserService.Commands;
using Totira.Bussiness.UserService.Common;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.Events;
using Totira.Bussiness.UserService.Extensions.BuisinessExtensions.Profile;
using Totira.Support.Application.Messages;
using Totira.Support.EventServiceBus;
using static Totira.Support.Application.Messages.IMessageHandler;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Commands;

public class DeleteTenantStudentDetailCommandHandler : IMessageHandler<DeleteTenantStudentDetailCommand>
{
    private readonly ILogger<DeleteTenantStudentDetailCommandHandler> _logger;
    private readonly IRepository<TenantEmployeeIncomes, Guid> _tenantEmployeeIncomesRepository;
    private readonly IRepository<TenantVerificationProfile, Guid> _tenantVerificationProfileRepository;
    private readonly IEventBus _bus;
    private readonly IContextFactory _contextFactory;
    private readonly ICommonFunctions _helper;
    private readonly IMessageService _messageService;

    public DeleteTenantStudentDetailCommandHandler(
        ILogger<DeleteTenantStudentDetailCommandHandler> logger,
        IRepository<TenantEmployeeIncomes, Guid> tenantEmployeeIncomesRepository,
        IRepository<TenantVerificationProfile, Guid> tenantVerificationProfileRepository,
        IEventBus bus,
        IContextFactory contextFactory,
        ICommonFunctions helper,
        IMessageService messageService)
    {
        _logger = logger;
        _tenantEmployeeIncomesRepository = tenantEmployeeIncomesRepository;
        _tenantVerificationProfileRepository = tenantVerificationProfileRepository;
        _helper = helper;
        _bus = bus;
        _contextFactory = contextFactory;
        _messageService = messageService;
    }

    public async Task HandleAsync(IContext context, Either<Exception, DeleteTenantStudentDetailCommand> command)
    {
        await command.MatchAsync(async cmd => {
            Guid? messageOutboxId = null;
            var tenantIncomes = await _tenantEmployeeIncomesRepository.GetByIdAsync(cmd.TenantId);
            if (tenantIncomes is null || tenantIncomes.StudentIncomes is null || !tenantIncomes.StudentIncomes.Any())
            {
                _logger.LogError("Student incomes for tenant {TenantId} not found.", cmd.TenantId);
                return;
            }

            var studyDetails = tenantIncomes.StudentIncomes.FirstOrDefault(income => income.Id == cmd.StudyId);
            if (studyDetails is null)
            {
                _logger.LogError("Student detail {StudyId} not found.", cmd.StudyId);
                return;
            }

            List<bool> enrollmentProofsResults = new();
            List<bool> studyPermitsOrVisasResults = new();
            List<bool> incomeProofsResults = new();

            foreach (var file in studyDetails.EnrollmentProofs)
            {
                var result = await _helper.DeleteFileAsync(file);
                enrollmentProofsResults.Add(result);
            }

            foreach (var file in studyDetails.StudyPermitsOrVisas)
            {
                var result = await _helper.DeleteFileAsync(file);
                studyPermitsOrVisasResults.Add(result);
            }

            foreach (var file in studyDetails.IncomeProofs)
            {
                var result = await _helper.DeleteFileAsync(file);
                incomeProofsResults.Add(result);
            }

            if (enrollmentProofsResults.All(deleted => deleted) &&
                studyPermitsOrVisasResults.All(deleted => deleted) &&
                incomeProofsResults.All(deleted => deleted))
            {
                tenantIncomes.StudentIncomes.Remove(studyDetails);
                _logger.LogInformation("Student detail {studyId} deleted.", cmd.StudyId);

                tenantIncomes.IsStudent = IfStillHavingStudentIncomesWithoutEmploymentIncomes(tenantIncomes);

                await _tenantEmployeeIncomesRepository.Update(tenantIncomes);
                _logger.LogInformation("Tenant incomes updated.");
            }
            else
                _logger.LogError("Student detail {StudyId} delete failed.", cmd.StudyId);

        if (tenantIncomes.EmployeeIncomes is not null
            && !tenantIncomes.EmployeeIncomes.Any(x => x.IsCurrentlyWorking)
            && tenantIncomes.StudentIncomes is not null
            && tenantIncomes.StudentIncomes.Count == 0)
        {
            // await _repositoryApplicationDetails.VerifyAndUpdateJiraAndVerifiedProfile(_contextFactory, _bus, cmd.TenantId);
            await _tenantVerificationProfileRepository.UnverifyTenant(cmd.TenantId, _contextFactory, _bus);
        }

            // Create Event
            var studentDetailDeletedEvent = new TenantStudentDetailDeletedEvent(cmd.TenantId, "Study Deleted");
            var notificationContext = _contextFactory.Create(string.Empty, context.CreatedBy);
            messageOutboxId = await _messageService.SendAsync(notificationContext, studentDetailDeletedEvent);
        }, ex => throw ex);
    }

    private static bool IfStillHavingStudentIncomesWithoutEmploymentIncomes(TenantEmployeeIncomes tenantIncomes) =>
        tenantIncomes.StudentIncomes is not null &&
        tenantIncomes.StudentIncomes.Any() &&
        (tenantIncomes.EmployeeIncomes is null || !tenantIncomes.EmployeeIncomes.Any());
}