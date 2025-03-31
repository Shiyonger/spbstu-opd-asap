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
    public async Task<IActionResult> Register(UserRegisterDto userRegister)
    {
        await _userService.Register(userRegister.Name, userRegister.Login, userRegister.Password, userRegister.Email,
            userRegister.Role, userRegister.GithubLink);

        return Ok();
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Login(UserLoginDto userLogin)
    {
        var token = await _userService.Login(userLogin.Login, userLogin.Password);
        
        HttpContext.Response.Cookies.Append("tasty-cookies", token);
        
        return Ok();
    }
}