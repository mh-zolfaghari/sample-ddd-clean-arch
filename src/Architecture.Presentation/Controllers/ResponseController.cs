namespace Architecture.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public sealed class ResponseController : ControllerBase
{
    [HttpGet("/api/responses/failure")]
    public IActionResult Failure()
        => Result.Failure(Error.Failure("Operation Failed", ErrorSeverity.Technical)).ToEndpointResponse();

    [HttpGet("/api/responses/forbidden")]
    public IActionResult Forbidden()
        => Result.Failure(Error.Forbidden("Access Denied", ErrorSeverity.Technical)).ToEndpointResponse();

    [HttpGet("/api/responses/unauthorized")]
    public IActionResult UnauthorizedRequest()
        => Result.Failure(Error.Unauthorized("Unauthorized", ErrorSeverity.Technical)).ToEndpointResponse();

    [HttpGet("/api/responses/validation")]
    public IActionResult Validation()
        => Result.Failure(Error.Validation("Item Validation", ErrorSeverity.Technical)).ToEndpointResponse();

    [HttpGet("/api/responses/conflict")]
    public IActionResult ConflictItem()
        => Result.Failure(Error.Conflict("Duplicated Item", ErrorSeverity.Technical)).ToEndpointResponse();

    [HttpGet("/api/responses/unprocessable-entity")]
    public IActionResult UnprocessableEntityItem()
        => Result.Failure(Error.UnprocessableEntity("Incorrect Item", ErrorSeverity.Technical)).ToEndpointResponse();

    [HttpGet("/api/responses/not-found")]
    public IActionResult NotFoundItem()
        => Result.Failure(Error.NotFound("Item Not found", ErrorSeverity.Technical)).ToEndpointResponse();

    [HttpGet("/api/responses/service-unavailable")]
    public IActionResult ServiceUnavailable()
        => Result.Failure(Error.ServiceUnavailable("Service Unavailable", ErrorSeverity.Technical)).ToEndpointResponse();

    [HttpGet("/api/responses/toomany-requests")]
    public IActionResult TooManyRequests()
        => Result.Failure(Error.TooManyRequests("TooMany Requests", ErrorSeverity.Technical)).ToEndpointResponse();

    [HttpGet("/api/responses/global-exception")]
    public IActionResult GlobalException()
        => throw new ApplicationException("Just for testing Sentry integration!");
}
