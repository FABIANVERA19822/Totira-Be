using Totira.Bussiness.UserService.DTO.Common;
using Totira.Support.Application.Commands;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Bussiness.UserService.Commands;

[RoutingKey(nameof(UpdateTenantStudentFinancialDetailCommand))]
public record UpdateTenantStudentFinancialDetailCommand(
    Guid TenantId,
    Guid StudyId,
    string UniversityOrInstitute,
    string Degree,
    bool IsOverseasStudent,
    List<string> DeletedEnrollmentProofs,
    List<TenantFileInfoDto> NewEnrollmentProofs,
    List<string> DeletedStudyPermitsOrVisas,
    List<TenantFileInfoDto> NewStudyPermitsOrVisas,
    List<string> DeletedIncomesProofs,
    List<TenantFileInfoDto> NewIncomesProofs) : ICommand;
