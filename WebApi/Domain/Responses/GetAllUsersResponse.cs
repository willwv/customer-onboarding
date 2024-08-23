namespace Domain.Responses
{
    public record GetAllUsersResponse(int? TotalPages, IList<UserResponse>? Users, IList<string>? Errors = null) : BaseResponse(Errors);
}
