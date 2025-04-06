using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SPbSTU.OPD.ASAP.API.Domain.Contracts;
using SPbSTU.OPD.ASAP.API.Dto;

namespace SPbSTU.OPD.ASAP.API.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController(IUsersService userService) : Controller
{
    private readonly IUsersService _userService = userService;

    [HttpGet]
    [Authorize]
    public IActionResult Get()
    {
        return Ok();
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Register(UserRegisterDto userRegister, CancellationToken ct)
    {
        await _userService.Register(userRegister.Name, userRegister.Login, userRegister.Password, userRegister.Email,
            userRegister.Role, userRegister.GithubLink, ct);

        return Ok();
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Login(UserLoginDto userLogin, CancellationToken ct)
    {
        var result = await _userService.Login(userLogin.Login, userLogin.Password, ct);
        
        if (!result.IsSuccessful)
            return BadRequest(new { message = result.ErrorMessage });
        
        HttpContext.Response.Cookies.Append("tasty-cookies", result.Token);
        
        return Ok();
    }
}