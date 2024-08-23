using Application.Commands;
using Application.Queries;
using Domain.Requests;
using Domain.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using static Domain.Utils.Useful;

namespace WebApi.Controllers
{
    [Route("api/v1/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize(Policy = Policies.ADMIN_ONLY)]
        [SwaggerOperation(Summary = "Returns all users limited by 5 users per request/page. Page starts in 0.")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Users", Type = typeof(GetAllUsersResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Description = "Users not found.")]
        public async Task<IActionResult> GetAllUsers([FromQuery] int page, CancellationToken cancellationToken)
        {
            var getAllUsersQuery = new GetAllUsersQuery(page);
            var response = await _mediator.Send(getAllUsersQuery, cancellationToken);

            return response.Errors != null ? NotFound(response.Errors) : Ok(response);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = Policies.USER_OR_ADMIN)]
        [SwaggerOperation(Summary = "Returns user by Id.")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "User was found for the specified Id.", Type = typeof(GetUserByIdResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Description = "User not found.", Type = typeof(IList<string>))]
        public async Task<IActionResult> GetUserById([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var getUserQuery = new GetUserQuery(id);
            var response = await _mediator.Send(getUserQuery, cancellationToken);

            return response.Errors != null ? NotFound(response.Errors) : Ok(response);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create a new user.")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "User created successfully.", Type = typeof(CreateUserResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "User email already exists.")]

        public async Task<IActionResult> CreateUser([FromForm] CreateUserRequest request, CancellationToken cancellationToken)
        {
            var createUserCommand = new CreateUserCommand(request);

            var response = await _mediator.Send(createUserCommand, cancellationToken);

            if(response.Errors != null)
            {
                return BadRequest(response.Errors);
            }
            else
            {
                return Ok(response);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Policy = Policies.USER_OR_ADMIN)]
        [SwaggerOperation(Summary = "Update user's information.")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "User updated.", Type = typeof(UpdateUserResponse))]
        public async Task<IActionResult> UpdateUser([FromRoute]Guid id, [FromForm] UpdateUserRequest request, CancellationToken cancellationToken)
        {
            var createUserCommand = new UpdateUserCommand(id, request.Name, ConvertFormFileToByteArray(request.Logo));

            var response = await _mediator.Send(createUserCommand, cancellationToken);

            return response.Errors != null ? BadRequest(response.Errors) : Ok(response.User);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = Policies.ADMIN_ONLY)]
        [SwaggerOperation(Summary = "Delete user by id.")]
        [SwaggerResponse(StatusCodes.Status204NoContent, Description = "User deleted.")]
        public async Task<IActionResult> DeleteUser([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var deleteUserCommand = new DeleteUserCommand(id);

            await _mediator.Send(deleteUserCommand, cancellationToken);

            return NoContent();
        }

        private static byte[]? ConvertFormFileToByteArray(IFormFile? file)
        {
            byte[]? logo = null;

            if (file != null)
            {
                using var memoryStream = new MemoryStream();
                file.CopyTo(memoryStream);
                logo = memoryStream.ToArray();
            }

            return logo;
        }
    }
}
