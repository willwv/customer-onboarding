namespace Domain.Requests
{
    public record UpdateAddressRequest
       (
        string Street,
        string Number,
        string Neighborhood,
        string City,
        string PostalCode,
        string State,
        string Complement
        );
}
