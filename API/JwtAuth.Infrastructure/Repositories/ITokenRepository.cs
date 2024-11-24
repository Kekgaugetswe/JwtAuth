using Microsoft.AspNetCore.Identity;

namespace JwtAuth.Infrastructure.Repositories;
public interface ITokenRepository
{
    string CreateJwtToken(IdentityUser user, List<string> roles);
}
