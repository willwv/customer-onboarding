using Domain.Models;
using Domain.Responses;
using MediatR;

namespace Application.Queries
{
    public record GetUserQuery(Guid Id) : IRequest<GetUserByIdResponse>;
}
