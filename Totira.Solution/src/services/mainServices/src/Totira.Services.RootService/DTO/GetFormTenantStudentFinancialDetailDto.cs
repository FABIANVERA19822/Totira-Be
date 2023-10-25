using Totira.Support.Application.Queries;

namespace Totira.Services.RootService.DTO;

public record GetFormTenantStudentFinancialDetailDto(Guid TenantId) : IQuery;
