namespace Domain.Models
{
    public record AddressDto(Guid? Id,
        Guid? UserId, 
        string Street, 
        string Number, 
        string Neighborhood, 
        string City, 
        string PostalCode, 
        string Country, 
        string Complement);
}
