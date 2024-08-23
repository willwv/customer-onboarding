namespace Domain.Responses
{
    public record AuthUserResponse(string? JwtToken, UserResponse? User, IList<string>? Errors = null) : BaseResponse(Errors);
}
