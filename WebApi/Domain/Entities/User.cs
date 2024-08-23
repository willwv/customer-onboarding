using Domain.Responses;

namespace Domain.Entities
{
    public class User
    {
        public User() { }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public byte[]? Logo { get; set; }
        public IList<Address>? Addresses { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }
        public string Role { get; set; }

        public UserResponse ToResponse() => new(
            Id,
            Name,
            Email,
            Role,
            Logo,
            Addresses?.Select(address => address.ToResponse()).ToList()
        );
    }
}
