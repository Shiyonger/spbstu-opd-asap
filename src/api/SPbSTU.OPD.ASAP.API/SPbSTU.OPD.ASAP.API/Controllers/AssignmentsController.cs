using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SPbSTU.OPD.ASAP.API.Domain.Contracts.Grpc;
using SPbSTU.OPD.ASAP.API.Domain.Enums;

namespace SPbSTU.OPD.ASAP.API.Controllers;

[ApiController]
[Route("[controller]")]
[EnableCors]
public class AssignmentsController(IAssignmentsGrpcService assignmentsService) : Controller
{
    private readonly IAssignmentsGrpcService _assignmentsService = assignmentsService;

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAssignments([FromQuery] long courseId, CancellationToken ct)
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        var userId = long.Parse(identity?.FindFirst("user_id")?.Value!);
        var role = Enum.Parse<Role>(identity?.FindFirst("Role")?.Value!);
        
        var result = await _assignmentsService.GetAssignments(userId, courseId, role, ct);
        return Ok(result);
    }
}