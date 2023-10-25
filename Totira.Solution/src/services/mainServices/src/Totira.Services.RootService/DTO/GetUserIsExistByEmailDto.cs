
namespace Totira.Services.RootService.DTO;

public record GetUserIsExistByEmailDto
{
    public bool IsExistUser { get; } = false;

    public GetUserIsExistByEmailDto(bool isExistUser)
    {
        this.IsExistUser = isExistUser;

    }
}



