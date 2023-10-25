using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Totira.Bussiness.UserService.DTO;

public record GetUserIsExistByEmailDto
{
    public bool IsExistUser { get; } = false;

    public GetUserIsExistByEmailDto(bool isExistUser)
    {
        this.IsExistUser = isExistUser;

    }
}



