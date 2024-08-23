namespace Domain.Responses
{
    public record UserAddressResponse(Guid? Id,
        Guid? UserId,
        string Street,
        string Number,
        string Neighborhood,
        string City,
        string PostalCode,
        string State,
        string Complement);
}
