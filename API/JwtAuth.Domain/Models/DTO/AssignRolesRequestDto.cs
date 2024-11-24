using System;

namespace JwtAuth.Domain.Models.DTO;

public class AssignRolesRequestDto
{
    public string Email { get; set; }
    public List<string> Roles { get; set; }
}
