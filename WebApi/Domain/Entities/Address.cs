using Domain.Responses;
using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class Address
    {
        public Address() { }
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        [JsonIgnore]
        public User? User { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string Neighborhood { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string State { get; set; }
        public string Complement { get; set; }

        public UserAddressResponse ToResponse() => new(
            Id,
            UserId,
            Street,
            Number,
            Neighborhood,
            City,
            PostalCode,
            State,
            Complement
            );
    }
}
