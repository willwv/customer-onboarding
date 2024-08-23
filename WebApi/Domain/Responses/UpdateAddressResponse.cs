namespace Domain.Responses
{
    public record UpdateAddressResponse(UserAddressResponse? Addres, IList<string>? Errors = null) : BaseResponse(Errors);
}
