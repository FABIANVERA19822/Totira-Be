namespace Totira.Services.RootService.DTO;

public class GetUserIsExistByEmailDto
{
    public bool IsExistUser { get; set; } = false;

    public bool IsExistApplicationProfileUser { get; set; } = false;
}