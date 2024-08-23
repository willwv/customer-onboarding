using Application.Commands;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Responses;
using MediatR;

namespace Application.CommandsHandler
{
    public class AuthenticateUserCommandHandler : IRequestHandler<AuthenticateUserCommand, AuthUserResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthService _authService;
        public AuthenticateUserCommandHandler(IAuthService authService, IUserRepository userRepository)
        {
            _authService = authService;
            _userRepository = userRepository;
        }

        public async Task<AuthUserResponse> Handle(AuthenticateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByEmailAsync(request.Email, cancellationToken);
            
            if (user == null)
            {
                return new AuthUserResponse(null, null, new List<string>() {"User not found."});
            }
            else
            {
                var jwtToken = IsValidUserCredentials(request.Password, user) ? 
                    _authService.GenerateJwtToken(user.Email, user.Role, user.Id) :
                    null;

                if (IsValidUserCredentials(request.Password, user))
                {
                    return new AuthUserResponse(jwtToken, user.ToResponse(), null);
                }
                else
                {
                    return new AuthUserResponse(null, user.ToResponse(), new List<string>() { "Invalid password" });
                }
            }
        }
        private static bool IsValidUserCredentials(string password, User? user) => (user != null && BCrypt.Net.BCrypt.Verify(password + user.Salt, user.PasswordHash));
    }
}
