using ArtExplorer.BLL.Dtos;
using ArtExplorer.BLL.Services.Interfaces;
using ArtExplorer.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ArtExplorer.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly ITokenService _tokenService;
    private readonly SignInManager<User> _signInManager;
    
    public AccountController(UserManager<User> userManager, ITokenService tokenService, SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _signInManager = signInManager;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.DisplayName == loginDto.Username);

        if (user == null)
        {
            return Unauthorized("Invalid username!");
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

        if (!result.Succeeded)
        {
            return Unauthorized("Username not found and/or password incorrect!");
        }

        return Ok(new UserDto()
        {
            Username = loginDto.Username,
            Token = _tokenService.CreateToken(user)
        });
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
