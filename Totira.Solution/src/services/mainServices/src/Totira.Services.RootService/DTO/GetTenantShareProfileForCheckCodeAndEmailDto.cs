namespace Totira.Services.RootService.DTO;

public record GetTenantShareProfileForCheckCodeAndEmailDto
{
    public Guid Id { get; set; } = Guid.Empty;
    public bool IsSuccess { get; set; } = false;
    public string ErrorMessage { get; set; } = string.Empty;

    public GetTenantShareProfileForCheckCodeAndEmailDto(bool isSuccess, Guid id)
    {
        this.IsSuccess = isSuccess;
        this.Id = id;
    }

    public GetTenantShareProfileForCheckCodeAndEmailDto(bool isSuccess, string errorMessage)
    {
        this.IsSuccess = isSuccess;
        this.ErrorMessage = errorMessage;
    }

    public GetTenantShareProfileForCheckCodeAndEmailDto()
    {

    }
}
