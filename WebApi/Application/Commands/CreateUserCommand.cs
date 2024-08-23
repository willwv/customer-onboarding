using Domain.Requests;
using Domain.Responses;
using MediatR;

namespace Application.Commands
{
    public record CreateUserCommand(CreateUserRequest NewUser) : IRequest<CreateUserResponse>;
}
