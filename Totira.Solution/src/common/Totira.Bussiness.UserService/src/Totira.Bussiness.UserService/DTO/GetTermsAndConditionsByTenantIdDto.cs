namespace Totira.Bussiness.UserService.DTO;

public record GetTermsAndConditionsByTenantIdDto(
    Guid TenantId,
    bool AcceptedTermsAndConditions);

