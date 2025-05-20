using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SPbSTU.OPD.ASAP.API.Domain.Contracts.Grpc;

namespace SPbSTU.OPD.ASAP.API.Controllers;

[ApiController]
[Route("[controller]")]
public class CoursesController(ICoursesGrpcService coursesService) : Controller
{
    private readonly ICoursesGrpcService _coursesService = coursesService;

    [HttpGet("[action]")]
    [Authorize]
    public async Task<IActionResult> GetCourses(CancellationToken ct)
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        var userId = long.Parse(identity?.FindFirst("user_id")?.Value!);

        var result = await _coursesService.GetCourses(userId, ct);
        return Ok(result);
    }
}