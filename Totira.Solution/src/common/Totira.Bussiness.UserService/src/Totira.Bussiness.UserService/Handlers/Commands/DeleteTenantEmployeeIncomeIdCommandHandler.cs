
namespace Totira.Bussiness.UserService.Handlers.Commands
{
    using Amazon.SimpleEmail.Model;
    using Microsoft.Extensions.Logging;
    using MongoDB.Driver;
    using System.Threading.Tasks;
    using Totira.Bussiness.UserService.Commands;
    using Totira.Bussiness.UserService.Domain;
    using Totira.Bussiness.UserService.Domain.Common;
    using Totira.Support.Application.Messages;
    using Totira.Support.CommonLibrary.Interfaces;
    using static Totira.Support.Application.Messages.IMessageHandler;
    using static Totira.Support.Persistance.IRepository;

    public class DeleteTenantEmployeeIncomeIdCommandHandler : IMessageHandler<DeleteTenantEmployeeIncomeIdCommand>
    {
        private readonly IS3Handler _s3Handler;
        private readonly IRepository<TenantEmployeeIncomes, Guid> _incomesTenantEmployeerepository;
        private readonly IRepository<TenantEmploymentReference, Guid> _tenantEmploymentReferenceRepository;
        private readonly ILogger<DeleteTenantEmployeeIncomeIdCommandHandler> _logger;
        public DeleteTenantEmployeeIncomeIdCommandHandler(
            IRepository<TenantEmployeeIncomes, Guid> repository,
            IRepository<TenantEmploymentReference, Guid> tenantEmploymentReferenceRepository,
            ILogger<DeleteTenantEmployeeIncomeIdCommandHandler> logger,
            IS3Handler s3Handler)
        {
            _incomesTenantEmployeerepository = repository;
            _tenantEmploymentReferenceRepository = tenantEmploymentReferenceRepository;
            _logger = logger;
            _s3Handler = s3Handler;
        }

        public async Task HandleAsync(IContext context, DeleteTenantEmployeeIncomeIdCommand message)
        {
            _logger.LogInformation("Starting to delete employment income {incomeId} detail of tenant {tenantId}",message.IncomeId,message.TenantId);
            await DeleteEmployeeIncomeInformationByIdAsync(message);
        }

        private async Task DeleteEmployeeIncomeInformationByIdAsync(DeleteTenantEmployeeIncomeIdCommand command)
        {
            var tenantIncomes = await _incomesTenantEmployeerepository.GetByIdAsync(command.TenantId);

            if (tenantIncomes is null || tenantIncomes.EmployeeIncomes is null)
            {
                _logger.LogInformation("Employee income information does not exist.");
                return;
            }
            var income = tenantIncomes.EmployeeIncomes.FirstOrDefault(income => income.Id == command.IncomeId);
            
            if (income is null)
                return; 

            await DeleteEmployeeReferencesAsync(command.TenantId);
            await DeleteEmployeeIncomesFilesAsync(income.Files);

            tenantIncomes.EmployeeIncomes.Remove(income);

            if (!tenantIncomes.EmployeeIncomes.Any())
                _logger.LogWarning("Last employment income was deleted.");

            tenantIncomes.IsStudent = IfDoesNotRemainEmploymentIncomesAndHasStudentIncomes(tenantIncomes);

            await _incomesTenantEmployeerepository.Update(tenantIncomes);
        }

        private async Task DeleteEmployeeReferencesAsync(Guid tenantId)
        {
            _logger.LogInformation("Starting to delete employment references of tenant {tenantId}", tenantId);

            var references = await _tenantEmploymentReferenceRepository.GetByIdAsync(tenantId);

            if (references is null)
            {
                _logger.LogInformation("Employee references information does not exist.");
                return;
            }

            references = default!;

            await _tenantEmploymentReferenceRepository.Update(references);
        }

        private async Task DeleteEmployeeIncomesFilesAsync(ICollection<TenantFile> files)
        {
            foreach (var file in files)
            {
                var isDeleted = await _s3Handler.DeleteObjectAsync(file.S3KeyName);

                if (!isDeleted)
                    _logger.LogError("File {fileName} delete failed.", file.FileName);
            }
        }

        public static bool IfDoesNotRemainEmploymentIncomesAndHasStudentIncomes(TenantEmployeeIncomes tenantIncomes) =>
            (tenantIncomes.EmployeeIncomes is null || tenantIncomes.EmployeeIncomes.Any()) &&
            (tenantIncomes.StudentIncomes is not null && tenantIncomes.StudentIncomes.Any());
    }
}

