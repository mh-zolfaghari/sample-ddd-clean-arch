namespace Architecture.Application.UseCases
{
    public record TestCommand
    {
        public string Name { get; set; } = default!;
        public string Family { get; set; } = default!;
    }

    public class TestCommandHandler()
    {
        public async Task<Result> Handle(TestCommand command, CancellationToken cancellationToken)
        {
            return await Task.Run(Result.Success);
        }
    }
}
