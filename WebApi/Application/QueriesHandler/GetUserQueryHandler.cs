using Application.Queries;
using Domain.Interfaces.Repositories;
using Domain.Responses;
using MediatR;

namespace Application.QueriesHandler
{
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, GetUserByIdResponse>
    {
        private readonly IUserRepository _userRepository;
        public GetUserQueryHandler(IUserRepository userRepository) 
        {
            _userRepository = userRepository;
        }

        public async Task<GetUserByIdResponse> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetCachedUserByIdAsync(request.Id, cancellationToken);

            if (user == null) 
            {
                return new(null, new List<string>() { "user not found" });
            }
            else
            {
                var addresses = user.Addresses?.Select(address => new UserAddressResponse(
                    address.Id,
                    address.UserId,
                    address.Street,
                    address.Number,
                    address.Neighborhood,
                    address.City,
                    address.PostalCode,
                    address.State,
                    address.Complement));

                return new(user.ToResponse());
            }
        }
    }
}
