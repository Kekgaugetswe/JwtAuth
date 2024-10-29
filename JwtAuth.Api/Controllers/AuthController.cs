using JwtAuth.Domain.Models.DTO;
using JwtAuth.Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JwtAuth.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ITokenRepository tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
            this.roleManager = roleManager;
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
                    var jwtToken = tokenRepository.CreateJwtToken(identityUser, roles.ToList());
                    var response = new LoginResponseDto()
                    {
                        Email = request.Email,
                        Roles = roles.ToList(),
                        Token = jwtToken
                    };

                    return Ok(response);


                }
            }
            ModelState.AddModelError("", "Email or password incorrect");

            return ValidationProblem(ModelState);


        }


        [HttpPost]
        [Route("assign-role")]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleRequestDto request)
        {
            var user = await userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                ModelState.AddModelError("", "User not found.");
                return ValidationProblem(ModelState);
            }

            var identityResult = await userManager.AddToRoleAsync(user, request.Role);
            if (identityResult.Succeeded)
            {
                return Ok("Role assigned successfully.");
            }

            foreach (var error in identityResult.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [Route("create-role")]
        public async Task<IActionResult> CreateRoleAsync([FromBody] CreateRoleRequestDto requestDto)
        {
            var roleExist = await roleManager.RoleExistsAsync(requestDto.RoleName);
            if (roleExist)
            {
                return BadRequest("Role already Exists");
            }

            var identityResult = await roleManager.CreateAsync(new IdentityRole(requestDto.RoleName));
            if (identityResult.Succeeded)
            {
                return Ok("role created Successfully");
            }

            foreach (var error in identityResult.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return ValidationProblem(ModelState);

        }

        [HttpGet]
        [Route("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = userManager.Users.ToList();
            var userList = new List<UserResponseDto>();

            foreach (var user in users)
            {
                var roles = await userManager.GetRolesAsync(user);
                userList.Add(new UserResponseDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    Roles = roles.ToList()
                });
            }

            return Ok(userList);
        }


        [HttpGet]
        [Route("user/{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var roles = await userManager.GetRolesAsync(user);
            var response = new UserResponseDto
            {
                Id = user.Id,
                Email = user.Email,
                Roles = roles.ToList()
            };

            return Ok(response);
        }


        [HttpGet]
        [Route("roles")]
        public IActionResult GetAllRoles()
        {
            var roles = roleManager.Roles.Select(r => new RoleResponseDto
            {
                Id = r.Id,
                Name = r.Name
            }).ToList();

            return Ok(roles);
        }

        [HttpGet]
        [Route("role/{id}")]
        public async Task<IActionResult> GetRoleById(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound("Role not found.");
            }

            var response = new RoleResponseDto
            {
                Id = role.Id,
                Name = role.Name
            };

            return Ok(response);
        }



    }
}
