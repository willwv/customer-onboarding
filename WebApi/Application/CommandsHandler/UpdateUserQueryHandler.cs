using Application.Commands;
using Domain.Interfaces.Repositories;
using Domain.Responses;
using MediatR;

namespace Application.CommandsHandler
{
    public class UpdateUserQueryHandler : IRequestHandler<UpdateUserCommand, UpdateUserResponse>
    {
        private readonly IUserRepository _userRepository;
        public UpdateUserQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UpdateUserResponse> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            if (!await _userRepository.CheckUserExistenceByIdAsync(request.Id, cancellationToken))
            {
                return new UpdateUserResponse(null, new List<string>() {"User not found."});
            }
            else
            {
                var updatedUser = await _userRepository.UpdateUserAsync(request.Id, request.Name, request.Logo, cancellationToken);

                return new UpdateUserResponse(updatedUser.ToResponse(), null);
            }
        }
    }
}
