using Domain.Responses;
using MediatR;

namespace Application.Commands
{
    public record UpdateAddressCommand(Guid Id,
         string Street,
         string Number,
         string Neighborhood,
         string City,
         string PostalCode,
         string State,
         string Complement) : IRequest<UpdateAddressResponse>;
}
