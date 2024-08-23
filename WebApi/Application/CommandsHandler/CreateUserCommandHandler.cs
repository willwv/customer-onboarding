using Application.Commands;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Responses;
using MediatR;
using Microsoft.AspNetCore.Http;
using static Domain.Utils.Useful;

namespace Application.CommandsHandler
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, CreateUserResponse>
    {
        private readonly IAuthService _authService;
        private readonly IUserRepository _userRepository;
        private readonly IAddressRepository _addressRepository;
        public CreateUserCommandHandler(IAuthService authService, IUserRepository userRepository, IAddressRepository addressRepository)
        {
            _authService = authService;
            _userRepository = userRepository;
            _addressRepository = addressRepository;
        }

        public async Task<CreateUserResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            if (await _userRepository.CheckUserExistenceByEmailAsync(request.NewUser.Email, cancellationToken))
            {
                return new CreateUserResponse(null, null, new List<string> { "User already exist." });
            }
            else 
            {
                var userResponse = await CreateUser(request, cancellationToken);

                var jwtToken = _authService.GenerateJwtToken(userResponse.Email, userResponse.Role, userResponse.Id);

                return new CreateUserResponse(jwtToken, userResponse);
            }
        }
        
        private async Task<UserResponse> CreateUser(CreateUserCommand request, CancellationToken cancellationToken)
        {
            byte[]? logo = ConvertToByteArray(request.NewUser.Logo);
            var salt = BCrypt.Net.BCrypt.GenerateSalt();
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.NewUser.Password + salt);

            var newUser = new User
            {
                Id = Guid.NewGuid(),
                Name = request.NewUser.Name,
                Logo = logo,
                Email = request.NewUser.Email,
                PasswordHash = passwordHash,
                Salt = salt,
                Role = Claims.SYS_USER
            };

            await _userRepository.CreateUserAsync(newUser, cancellationToken);

            return new UserResponse(newUser.Id, newUser.Name, newUser.Email, newUser.Role, newUser.Logo, null);
        }

        private static byte[]? ConvertToByteArray(IFormFile? file)
        {
            if (file == null || file.Length == 0)
                return null;

            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}
