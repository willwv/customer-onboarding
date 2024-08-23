using Application.Commands;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Responses;
using MediatR;

namespace Application.CommandsHandler
{
    public class CreateAddressCommandHandler : IRequestHandler<CreateAddressCommand, CreateAddressResponse>
    {
        public readonly IAddressRepository _addressRepository;
        public readonly IUserRepository _userRepository;
        public CreateAddressCommandHandler(IAddressRepository addressRepository, IUserRepository userRepository) 
        {
            _addressRepository = addressRepository;
            _userRepository = userRepository;
        }

        public async Task<CreateAddressResponse> Handle(CreateAddressCommand request, CancellationToken cancellationToken)
        {
            var userExist = await _userRepository.CheckUserExistenceByIdAsync(request.UserId, cancellationToken);
            
            if (!userExist)
            {
                return new CreateAddressResponse(null, new List<string> { "User didn't exist." });
            }
            else
            {
                var createdAddress = await CreateAddressAsync(request, cancellationToken);
                return new CreateAddressResponse(createdAddress, null);
            }
        }

        private async Task<UserAddressResponse> CreateAddressAsync(CreateAddressCommand request, CancellationToken cancellationToken)
        {
            var newAddress = new Address
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                Street = request.Street,
                Number = request.Number,
                Neighborhood = request.Neighborhood,
                City = request.City,
                PostalCode = request.PostalCode,
                State = request.State,
                Complement = request.Complement
            };

            await _addressRepository.CreateAddressAsync(newAddress, cancellationToken);

            return newAddress.ToResponse();
        }
    }
}
