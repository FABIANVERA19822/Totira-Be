namespace Totira.Services.RootService.DTO;

public record GetTenantShareProfileForCheckCodeAndEmailDto
{
    public Guid Id { get; set; } = Guid.Empty;
    public bool IsSuccess { get; set; } = false;
    public string ErrorMessage { get; set; } = string.Empty;
    public string TypeOfContact { get; set; } = string.Empty;

    public GetTenantShareProfileForCheckCodeAndEmailDto(bool isSuccess, Guid id, string typeOfContact)
    {
        this.IsSuccess = isSuccess;
        this.Id = id;
        this.TypeOfContact = typeOfContact;
    }

    public GetTenantShareProfileForCheckCodeAndEmailDto(bool isSuccess, string errorMessage, string typeOfContact)
    {
        this.IsSuccess = isSuccess;
        this.ErrorMessage = errorMessage;
        this.TypeOfContact = typeOfContact;
    }

    public GetTenantShareProfileForCheckCodeAndEmailDto()
    {

    }
}
