using Totira.Services.RootService.DTO.Common;

namespace Totira.Services.RootService.DTO;

public record GetTenantStudentDetailByIdDto(
    Guid TenantId,
    Guid StudyId,
    string UniversityOrInstitute,
    string Degree,
    bool IsOverseasStudent,
    IList<TenantFileDisplayDto> EnrollmentProofs,
    IList<TenantFileDisplayDto> StudyPermitOrVisa,
    IList<TenantFileDisplayDto> IncomeProofs);