using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VisionCare.Application.Commands.Users;
using VisionCare.Application.Queries.Users;
using VisionCare.WebAPI.Responses;

namespace VisionCare.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Authorize(Policy = "StaffOrAdmin")]
    public async Task<IActionResult> GetAllUsers()
    {
        var query = new GetAllUsersQuery();
        var result = await _mediator.Send(query);
        return Ok(ApiResponse<IEnumerable<object>>.Ok(result));
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchUsers(
        [FromQuery] string? keyword,
        [FromQuery] int? roleId,
        [FromQuery] string? status,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? sortBy = null,
        [FromQuery] bool desc = false
    )
    {
        var query = new SearchUsersQuery
        {
            Keyword = keyword,
            RoleId = roleId,
            Status = status,
            Page = page,
            PageSize = pageSize,
            SortBy = sortBy,
            Desc = desc,
        };

        var result = await _mediator.Send(query);
        return Ok(
            PagedResponse<object>.Ok(result.Items, result.TotalCount, result.Page, result.PageSize)
        );
    }

    [HttpGet("{id}")]
    [Authorize(Policy = "OwnProfileOrAdmin")]
    public async Task<IActionResult> GetUserById(int id)
    {
        var query = new GetUserByIdQuery { Id = id };
        var result = await _mediator.Send(query);
        return Ok(ApiResponse<object>.Ok(result));
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(
            nameof(GetUserById),
            new { id = result.Id },
            ApiResponse<object>.Ok(result)
        );
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserCommand command)
    {
        command.Id = id;
        var result = await _mediator.Send(command);
        return Ok(ApiResponse<object>.Ok(result));
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var command = new DeleteUserCommand { Id = id };
        var result = await _mediator.Send(command);

        if (!result)
        {
            return NotFound(ApiResponse<object>.Fail($"User with ID {id} not found."));
        }

        return Ok(ApiResponse<object>.Ok(null, "Deleted"));
    }
}
