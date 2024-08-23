using Application.Commands;
using Domain.Interfaces.Services;
using Domain.Requests;
using Domain.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace WebApi.Controllers
{
    [Route("api/v1/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IAuthService _authService;

        public AuthController(IMediator mediator, IAuthService authService)
        {
            _mediator = mediator;
            _authService = authService;
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Authenticate user.")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "User authenticated.", Type = typeof(AuthUserResponse))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Invalid password.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, Description = "User not found.")]
        public async Task<IActionResult> GetUserData([FromBody] AuthUserRequest request, CancellationToken cancellationToken)
        {
            var authUserCommand = new AuthenticateUserCommand(request.Email, request.Password);
            var userAuth = await _mediator.Send(authUserCommand, cancellationToken);

            if (userAuth.Errors != null && userAuth.User != null)
            {
                return BadRequest(userAuth.Errors);
            }
            else if (userAuth.Errors != null)
            {
                return NotFound(userAuth.Errors);
            }
            else
            {
                return Ok(userAuth);
            }
        }
    }
}
