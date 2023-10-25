using Totira.Support.Application.Queries;

namespace Totira.Bussiness.UserService.Queries;

public record QueryDownloadTenantIncomeFile(
    string FileName,
    Guid TenantId,
    Guid? IncomeId) : IQuery;
