using System;

namespace JwtAuth.Domain.Models.DTO;

public class AssignRoleRequestDto
{
    public string Email { get; set; }
    public string Role { get; set; }

}
