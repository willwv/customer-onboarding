using Application.Queries;
using Domain.Interfaces.Repositories;
using Domain.Responses;
using Domain.Utils;
using MediatR;

namespace Application.QueriesHandler
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, GetAllUsersResponse>
    {
        private readonly IUserRepository _userRepository;

        public GetAllUsersQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<GetAllUsersResponse> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var allUsers = await _userRepository.GetAllUsersAsync(request.Page, cancellationToken);

            if (allUsers == null)
            {
                return new GetAllUsersResponse(null, null, new List<string>() { "No users found." });
            }
            else
            {
                var usersCount = await _userRepository.CountAllUsers(cancellationToken);
                int maxPages = (int)Math.Ceiling((double)usersCount / Useful.USERS_PER_PAGE);

                var users = allUsers.Select(user => new UserResponse(user.Id, user.Name, user.Email, user.Role, user.Logo, user.Addresses?.Select(addr => addr.ToResponse()).ToList())).ToList();
                return new GetAllUsersResponse(maxPages, users, null);
            }
        }
    }
}
