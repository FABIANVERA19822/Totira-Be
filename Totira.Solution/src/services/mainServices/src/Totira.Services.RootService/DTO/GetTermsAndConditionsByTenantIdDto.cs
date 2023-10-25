namespace Totira.Services.RootService.DTO;

public record GetTermsAndConditionsByTenantIdDto(
    Guid TenantId,
    bool AcceptedTermsAndConditions);
