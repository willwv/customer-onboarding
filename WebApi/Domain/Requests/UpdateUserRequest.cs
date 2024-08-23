using Microsoft.AspNetCore.Http;

namespace Domain.Requests
{
    public record UpdateUserRequest(string Name, IFormFile? Logo);
}
