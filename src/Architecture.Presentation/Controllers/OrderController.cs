using Architecture.Application.UseCases.Orders.Create;
using Architecture.Application.UseCases.Orders.Filter;

namespace Architecture.Presentation.Controllers
{
    public sealed class OrderController
        (
            IMediator mediator,
            IServiceScopeFactory serviceScopeFactory
        ) : BaseController(serviceScopeFactory)
    {
        [HttpGet("/api/orders/create")]
        public async Task<IActionResult> Create(CancellationToken cancellationToken = default)
        {
            //throw new NotImplementedException("This is a test message for handling technical error.");
            var result = await mediator.SendAsync(new CreateOrderCommand(), cancellationToken);
            return HandleResponse(result);
        }


        //[HttpGet("/api/orders/delete/{id:guid}")]
        //public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken = default)
        //{
        //    var result = await mediator.SendAsync(new DeleteOrderCommand(id), cancellationToken);
        //    return result.ToEndpointResponse();
        //}


        //[HttpGet("/api/orders/submit/{id:guid}")]
        //public async Task<IActionResult> Submit(Guid id, CancellationToken cancellationToken = default)
        //{
        //    var result = await mediator.SendAsync(new SubmitOrderCommand(id), cancellationToken);
        //    return result.ToEndpointResponse();
        //}


        [HttpGet("/api/orders/list")]
        public async Task<IActionResult> List([FromQuery] FilterOrdersQuery request, CancellationToken cancellationToken = default)
        {
            var result = await mediator.SendAsync(request, cancellationToken);
            return HandleResponse(result);
        }

        //[HttpGet("/api/test")]
        //public async Task<IActionResult> Test(CancellationToken cancellationToken = default)
        //{
        //    // Error CS1503 'Argument 1: cannot convert from 'Architecture.Application.Orders.TestCommand' to 'Architecture.Shared.Commons.CQRS.Contracts.IRequest<TResponse>'
        //    //var result = await mediator.SendAsync(new TestCommand { Name = "Ali", Family = "Alavi" }, cancellationToken);
        //    //return result.ToEndpointResponse();

        //    return NoContent();
        //}
    }
}
