using Microsoft.Extensions.Logging;
using Totira.Bussiness.UserService.Common;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.Domain.Common;
using Totira.Bussiness.UserService.DTO.Files;
using Totira.Bussiness.UserService.Queries;
using Totira.Support.Application.Queries;
using Totira.Support.CommonLibrary.Interfaces;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Queries;

public class DownloadTenantIncomeFileQueryHandler : IQueryHandler<QueryDownloadTenantIncomeFile, DownloadTenantFileDto>
{
    private readonly ILogger<DownloadTenantIncomeFileQueryHandler> _logger;
    private readonly IRepository<TenantEmployeeIncomes, Guid> _tenantEmployeeIncomesRepository;
    private readonly ICommonFunctions _helper;

    public DownloadTenantIncomeFileQueryHandler(
        ILogger<DownloadTenantIncomeFileQueryHandler> logger,
        IRepository<TenantEmployeeIncomes, Guid> tenantEmployeeIncomesRepository,
        ICommonFunctions helper)
    {
        _logger = logger;
        _tenantEmployeeIncomesRepository = tenantEmployeeIncomesRepository;
        _helper = helper;
    }

    public async Task<DownloadTenantFileDto> HandleAsync(QueryDownloadTenantIncomeFile query)
    {
        var tenant = await _tenantEmployeeIncomesRepository.GetByIdAsync(query.TenantId);
        
        if (tenant is null)
            return new("application/octet-stream", "", Array.Empty<byte>());

        _logger.LogInformation("Tenant incomes {tenantId}", query.TenantId);

        Func<TenantFile, bool> isRequestedFile = (file => file.FileName == query.FileName);
        DownloadTenantFileDto response = new(string.Empty, string.Empty, Array.Empty<byte>());

        if (tenant.EmployeeIncomes is not null && tenant.EmployeeIncomes.Any())
        {
            var income = tenant.EmployeeIncomes.FirstOrDefault(x => x.Id == query.IncomeId);
            if (income is not null && income.Files.Any(isRequestedFile))
            {
                var info = income.Files.First(isRequestedFile);
                response = await _helper.DownloadFileAsync(info);
            }
        }

        if (tenant.StudentIncomes is not null && tenant.StudentIncomes.Any())
        {
            var income = tenant.StudentIncomes.FirstOrDefault(x => x.Id == query.IncomeId);
            var files = new List<TenantFile>();
            if (income is not null)
            {
                files.AddRange(income.EnrollmentProofs);
                files.AddRange(income.StudyPermitsOrVisas);
                files.AddRange(income.IncomeProofs);

                if (files.Any(isRequestedFile))
                {
                    var info = files.First(isRequestedFile);
                    response = await _helper.DownloadFileAsync(info);
                }
            }
        }

        return response;
    }
}
