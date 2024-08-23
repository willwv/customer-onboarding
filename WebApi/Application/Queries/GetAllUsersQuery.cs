using Domain.Models;
using Domain.Responses;
using MediatR;

namespace Application.Queries
{
    public record GetAllUsersQuery(int Page) : IRequest<GetAllUsersResponse>;
}
