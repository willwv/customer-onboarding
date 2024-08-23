using Application.Commands;
using Domain.Interfaces.Repositories;
using Domain.Responses;
using MediatR;

namespace Application.CommandsHandler
{
    public class UpdateAddressCommandHandler : IRequestHandler<UpdateAddressCommand, UpdateAddressResponse>
    {
        private readonly IAddressRepository _addressRepository;
        public UpdateAddressCommandHandler(IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }

        public async Task<UpdateAddressResponse> Handle(UpdateAddressCommand request, CancellationToken cancellationToken)
        {
            if (!await _addressRepository.CheckAddressExistenceByIdAsync(request.Id, cancellationToken))
            {
                return new UpdateAddressResponse(null, new List<string> { "Address don't exist." });
            }
            else
            {
                var updatedAddress = await _addressRepository.UpdateAddressAsync(request.Id, 
                    request.Street, 
                    request.Number,
                    request.Neighborhood,
                    request.City,
                    request.PostalCode,
                    request.State,
                    request.Complement,
                    cancellationToken);

                return new UpdateAddressResponse(updatedAddress.ToResponse(), null);
            }
        }
    }
}
