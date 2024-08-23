namespace Domain.Requests
{
    public record CreateAddressRequest(Guid UserId,
        string Street,
        string Number,
        string Neighborhood,
        string City,
        string PostalCode,
        string State,
        string Complement);
}
