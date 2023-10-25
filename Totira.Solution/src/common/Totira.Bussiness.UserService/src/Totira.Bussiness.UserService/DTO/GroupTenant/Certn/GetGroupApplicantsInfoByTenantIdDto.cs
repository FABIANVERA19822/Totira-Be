namespace Totira.Bussiness.UserService.DTO.GroupTenant.Certn;

public class GetGroupApplicantsInfoByTenantIdDto
{
    /// <summary>
    /// Indicates if all, almost or none applcants info is completed
    /// </summary>
    public string? Completed { get; set; }
    public List<TenantApplicantDto> Applicants { get; set; } = new();
}

public enum GroupApplicantCompletedInfo
{
    All = 1,
    Almost,
    None,
}

public class TenantApplicantDto
{
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public bool IsMainTenant { get; set; }
    public bool IsGuarantor { get; set; }
    /// <summary>
    /// Indicates if status of current applicant info is completed, missing basic info or missing contact info
    /// </summary>
    public string? Status { get; set; }
    public GetTenantInformationForCertnApplicationDto? Information { get; set; }
}

public enum TenantApplicantInfoStatus
{
    Complete = 1,
    MissingBasic,
    MissingContact,
}