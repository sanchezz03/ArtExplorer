using ArtExplorer.BLL.Dtos;
using ArtExplorer.BLL.Services.Interfaces;
using ArtExplorer.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ArtExplorer.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly ITokenService _tokenService;
    
    public AccountController(UserManager<User> userManager, ITokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new User()
            {
                DisplayName = registerDto.Username,
                UserName = registerDto.Username,
                Email = registerDto.Email,
            };

            var createdUser = await _userManager.CreateAsync(user, registerDto.Password);

            if (createdUser.Succeeded)
            {
                return Ok(new UserDto()
                {
                    Username = user.UserName,
                    Token = _tokenService.CreateToken(user)
                });
            }
            else
            {
                return BadRequest(createdUser.Errors);    
            }
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
