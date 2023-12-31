using Totira.Bussiness.UserService.DTO.Common;

namespace Totira.Bussiness.UserService.DTO;

public record GetTenantStudentDetailByIdDto(
    Guid TenantId,
    Guid StudyId,
    string UniversityOrInstitute,
    string Degree,
    bool IsOverseasStudent,
    IList<TenantFileDisplayDto> EnrollmentProof,
    IList<TenantFileDisplayDto> StudyPermitOrVisa,
    IList<TenantFileDisplayDto> IncomeProofs);