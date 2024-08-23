namespace Domain.Responses
{
    public record CreateUserResponse(string? JwtToken, UserResponse? User, IList<string>? Errors = null) : BaseResponse(Errors);
}
