using Totira.Bussiness.UserService.DTO.Common;
using Totira.Support.Application.Commands;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Bussiness.UserService.Commands;

[RoutingKey(nameof(CreateTenantStudentFinancialDetailCommand))]
public record CreateTenantStudentFinancialDetailCommand(
    Guid TenantId,
    string UniversityOrInstitute,
    string Degree,
    bool IsOverseasStudent,
    List<FileInfoDto> EnrollmentProofs,
    List<FileInfoDto>? StudyPermitsOrVisas,
    List<FileInfoDto> IncomeProofs) : ICommand;