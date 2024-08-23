using MediatR;

namespace Application.Commands
{
    public record DeleteAddressCommand(Guid Id) : IRequest;
}
