using JwtAuth.Domain.Models.DTO;
using JwtAuth.Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JwtAuth.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class AuthController: ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
        }


        //Create a user
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            //Create idendityUser object

            var user = new IdentityUser
            {
                UserName = request.Email?.Trim(),
                Email = request.Email?.Trim()
            };

            var identityResult = await userManager.CreateAsync(user, request.Password);

            if (identityResult.Succeeded)
            {
                // Add role to user {Reader}

                identityResult = await userManager.AddToRoleAsync(user, "Reader");
                if (identityResult.Succeeded)
                {
                    return Ok();
                }


            }
            else
            {
                if (identityResult.Errors.Any())
                {
                    foreach (var error in identityResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var identityUser = await userManager.FindByEmailAsync(request.Email);

            if (identityUser is not null)
            {
                // checked Password

                var checkPassword = await userManager.CheckPasswordAsync(identityUser, request.Password);

                if (checkPassword)
                {
                    var roles = await userManager.GetRolesAsync(identityUser);
                    // Created a token
                    var jwtToken=tokenRepository.CreateJwtToken(identityUser,roles.ToList());
                    var response = new LoginResponseDto(){
                        Email = request.Email,
                        Roles =  roles.ToList(),
                        Token = jwtToken
                    };

                    return Ok(response);


                }
            }
            ModelState.AddModelError("", "Email or password incorrect");

            return ValidationProblem(ModelState);


        }

    }
}
