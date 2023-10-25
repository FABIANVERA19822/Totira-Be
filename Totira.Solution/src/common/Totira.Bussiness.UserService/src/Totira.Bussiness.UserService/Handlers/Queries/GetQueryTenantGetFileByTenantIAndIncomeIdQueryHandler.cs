using Microsoft.Extensions.Logging;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.DTO;
using Totira.Bussiness.UserService.Queries;
using Totira.Support.Application.Queries;
using Totira.Support.CommonLibrary.Interfaces;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Queries
{
    public class GetQueryTenantGetFileByTenantIAndIncomeIdQueryHandler : IQueryHandler<QueryTenantGetFileByTenantIAndIncomeId, GetIncomeFilesDto>
    {
        private readonly ILogger<GetQueryTenantGetFileByTenantIAndIncomeIdQueryHandler> _logger;
        private readonly IRepository<TenantEmployeeIncomes, Guid> _tenantEmployeeIncomesRepository;
        private readonly IS3Handler _S3Handler;

        public GetQueryTenantGetFileByTenantIAndIncomeIdQueryHandler(
            ILogger<GetQueryTenantGetFileByTenantIAndIncomeIdQueryHandler> logger,
            IRepository<TenantEmployeeIncomes, Guid> tenantEmployeeIncomesRepository,
            IS3Handler S3Handler)
        {
            _tenantEmployeeIncomesRepository = tenantEmployeeIncomesRepository;
            _logger = logger;
            _S3Handler = S3Handler;
        }

        public async Task<GetIncomeFilesDto> HandleAsync(QueryTenantGetFileByTenantIAndIncomeId query)
        {
            _logger.LogInformation($"Getting files for income {query.IncomeId} for tenant {query.TenantId}");

            List<DTO.File> files = new List<DTO.File>();

            GetIncomeFilesDto objResult = new GetIncomeFilesDto()
            {
                IncomeId = query.IncomeId
            };

            var result = await _tenantEmployeeIncomesRepository.GetByIdAsync(query.TenantId);
            if (result.EmployeeIncomes != null && result.EmployeeIncomes.Any())
            {
                var filesAux = result.EmployeeIncomes.FirstOrDefault(ei => ei.Id == query.IncomeId)?.Files ?? new List<Domain.Common.TenantFile>();
                foreach (var file in filesAux)
                {
                    var fileDto = new DTO.File();

                    fileDto.Content = await _S3Handler.DownloadSingleFileAsync(file.S3KeyName);
                    fileDto.FileName = file.FileName;
                    fileDto.Extension = file.Extension;
                    files.Add(fileDto);
                }
            }

            if (result.StudentIncomes != null && result.StudentIncomes.Any())
            {
                var filesAux = result.StudentIncomes.FirstOrDefault(ei => ei.Id == query.IncomeId)?.IncomeProofs ?? new List<Domain.Common.TenantFile>();
                foreach (var file in filesAux)
                {
                    var fileDto = new DTO.File();

                    fileDto.Content = await _S3Handler.DownloadSingleFileAsync(file.S3KeyName);
                    fileDto.FileName = file.FileName;
                    fileDto.Extension = file.Extension;
                    files.Add(fileDto);
                }
                filesAux = result.StudentIncomes.FirstOrDefault(ei => ei.Id == query.IncomeId)?.EnrollmentProofs ?? new List<Domain.Common.TenantFile>();
                foreach (var file in filesAux)
                {
                    var fileDto = new DTO.File();

                    fileDto.Content = await _S3Handler.DownloadSingleFileAsync(file.S3KeyName);
                    fileDto.FileName = file.FileName;
                    fileDto.Extension = file.Extension;
                    files.Add(fileDto);
                }

                filesAux = result.StudentIncomes.FirstOrDefault(ei => ei.Id == query.IncomeId)?.StudyPermitsOrVisas ?? new List<Domain.Common.TenantFile>();
                foreach (var file in filesAux)
                {
                    var fileDto = new DTO.File();

                    fileDto.Content = await _S3Handler.DownloadSingleFileAsync(file.S3KeyName);
                    fileDto.FileName = file.FileName;
                    fileDto.Extension = file.Extension;
                    files.Add(fileDto);
                }
            }

            objResult.Files = files;

            return objResult;
        }
    }
}