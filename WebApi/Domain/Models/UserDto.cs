using Domain.Entities;

namespace Domain.Models
{
    public record UserDto(
        Guid Id,
        string Name,
        string Email,
        byte[]? Logo,
        string Role,
        DateTime? CreatedAt,
        IList<AddressDto>? AddressesDto
        );
}
