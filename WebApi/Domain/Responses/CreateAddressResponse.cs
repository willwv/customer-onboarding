namespace Domain.Responses
{
    public record CreateAddressResponse(UserAddressResponse? Addres, IList<string>? Errors = null) : BaseResponse(Errors);
}
