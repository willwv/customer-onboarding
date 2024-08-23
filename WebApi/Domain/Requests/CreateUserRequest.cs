using Microsoft.AspNetCore.Http;

namespace Domain.Requests
{
    public record CreateUserRequest(string Email, string Password, string Name, IFormFile? Logo);
}
