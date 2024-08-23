using Domain.Entities;
using Domain.Models;

namespace Domain.Interfaces.Services
{
    public interface IAuthService
    {
        string GenerateJwtToken(string email, string permission, Guid id);
    }
}
