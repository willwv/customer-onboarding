namespace Domain.Responses
{
    public record GetUserByIdResponse(UserResponse? User, IList<string>? Errors = null) : BaseResponse(Errors);
}
