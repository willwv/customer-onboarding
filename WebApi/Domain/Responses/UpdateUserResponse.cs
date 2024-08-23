namespace Domain.Responses
{
    public record UpdateUserResponse(UserResponse User, IList<string>? Errors = null) : BaseResponse(Errors);
}
