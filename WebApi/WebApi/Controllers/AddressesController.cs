using Application.Commands;
using Domain.Models;
using Domain.Requests;
using Domain.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using static Domain.Utils.Useful;

namespace WebApi.Controllers
{
    [Route("api/v1/addresses")]
    [ApiController]
    public class AddressesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AddressesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Policy = Policies.USER_OR_ADMIN)]
        [SwaggerOperation(Summary = "Create a new address for an user.")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Address Created.", Type = typeof(CreateAddressResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Description = "User not found.")]
        public async Task<IActionResult> GetUserData([FromBody] CreateAddressRequest request, CancellationToken cancellationToken)
        {
            var addressCommand = new CreateAddressCommand(request.UserId,
                request.Street,
                request.Number, 
                request.Neighborhood, 
                request.City, 
                request.PostalCode,
                request.State,
                request.Complement);

            var address = await _mediator.Send(addressCommand, cancellationToken);

            return address != null ? Ok(address) : NotFound();
        }

        [HttpPut("{id}")]
        [Authorize(Policy = Policies.USER_OR_ADMIN)]
        [SwaggerOperation(Summary = "Update address information.")]
        [SwaggerResponse(StatusCodes.Status200OK, Description = "Address updated.", Type = typeof(UpdateAddressResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Description = "Address not found.")]
        public async Task<IActionResult> UpdateUser([FromRoute] Guid id, [FromBody] UpdateAddressRequest request, CancellationToken cancellationToken)
        {
            var updateAddress = new UpdateAddressCommand(
                id,
                request.Street,
                request.Number,
                request.Neighborhood,
                request.City,
                request.PostalCode,
                request.State,
                request.Complement
                );

            var updatedAddress = await _mediator.Send(updateAddress, cancellationToken);

            return updatedAddress != null ? Ok(updatedAddress) : NotFound();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = Policies.USER_OR_ADMIN)]
        [SwaggerOperation(Summary = "Delete Address.")]
        [SwaggerResponse(StatusCodes.Status204NoContent, Description = "Address deleted.")]
        public async Task<IActionResult> GetUserData([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var deleteAddressCommand = new DeleteAddressCommand(id);

            await _mediator.Send(deleteAddressCommand, cancellationToken);

            return NoContent();
        }
    }
}
