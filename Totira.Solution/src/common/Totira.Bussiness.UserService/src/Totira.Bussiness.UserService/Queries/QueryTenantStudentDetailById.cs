
using Totira.Support.Application.Queries;

namespace Totira.Bussiness.UserService.Queries;

public record QueryTenantStudentDetailById(
    Guid TenantId,
    Guid StudyId) : IQuery;