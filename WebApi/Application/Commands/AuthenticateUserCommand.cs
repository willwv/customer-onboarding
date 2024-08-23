using Domain.Models;
using Domain.Responses;
using MediatR;

namespace Application.Commands
{
    public record AuthenticateUserCommand(string Email, string Password) : IRequest<AuthUserResponse>;
}
