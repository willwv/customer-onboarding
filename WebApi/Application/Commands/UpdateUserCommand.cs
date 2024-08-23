using Domain.Models;
using Domain.Responses;
using MediatR;

namespace Application.Commands
{
    public record UpdateUserCommand(Guid Id, string Name, byte[]? Logo) : IRequest<UpdateUserResponse>;
}
