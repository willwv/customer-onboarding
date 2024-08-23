using Domain.Responses;
using MediatR;

namespace Application.Commands
{
    public record CreateAddressCommand(Guid UserId,
        string Street, 
        string Number, 
        string Neighborhood, 
        string City,
        string PostalCode, 
        string State, 
        string Complement) : IRequest<CreateAddressResponse>;
}
