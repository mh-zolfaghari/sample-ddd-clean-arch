using Architecture.Presentation.Commons.Problem;

namespace Architecture.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public abstract class BaseController(IServiceScopeFactory ServiceScopeFactory) : ControllerBase
{
    protected IActionResult HandleResponse(Result result)
            => CreateProblemResult(result.Error!);

    protected IActionResult HandleResponse<T>(Result<T> result)
            => result.IsSuccess
                ? Ok(result.Data)
                : CreateProblemResult(result.Error!);

    private IActionResult CreateProblemResult(Error error)
    {
        using IServiceScope Scope = ServiceScopeFactory.CreateScope();
        ApplicationProblemDetailsFactory applicationProblemDetailsFactory = Scope.ServiceProvider.GetService<ApplicationProblemDetailsFactory>()!;
        switch (error.Status)
        {
            case ErrorStatus.Validation:
                return BadRequest();
            case ErrorStatus.None:
            case ErrorStatus.Unauthorized:
            case ErrorStatus.Forbidden:
            case ErrorStatus.NotFound:
            case ErrorStatus.Conflict:
            case ErrorStatus.UnprocessableEntity:
            case ErrorStatus.Internal:
            default:
                {
                    var appProblem = applicationProblemDetailsFactory.Create(error);
                    return new ObjectResult(appProblem);
                }
        }
    }
}
