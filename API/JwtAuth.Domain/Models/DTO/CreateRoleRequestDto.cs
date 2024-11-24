using System;

namespace JwtAuth.Domain.Models.DTO;

public class CreateRoleRequestDto
{
    public string RoleName  {get; set;} = string.Empty;
}
