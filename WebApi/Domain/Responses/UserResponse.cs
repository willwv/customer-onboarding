namespace Domain.Responses
{
    public record UserResponse
    {
        public UserResponse(Guid id, string name, string email, string role, byte[]? logo, IList<UserAddressResponse>? addresses = null)
        {
            Id = id; 
            Name = name; 
            Email = email;
            Role = role;
            Logo = logo != null ? Convert.ToBase64String(logo) : null;
            Addresses = addresses;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string? Logo { get; set; }
        public IList<UserAddressResponse>? Addresses { get; set; }
    }
}
