using Application.Commands;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.CommandsHandler
{
    public class DeleteAddressCommandHandler : IRequestHandler<DeleteAddressCommand>
    {
        private readonly IAddressRepository _addressRepository;
        public DeleteAddressCommandHandler(IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }

        public async Task Handle(DeleteAddressCommand request, CancellationToken cancellationToken)
        {
            await _addressRepository.DeleteAddressAsync(request.Id, cancellationToken);
        }
    }
}
