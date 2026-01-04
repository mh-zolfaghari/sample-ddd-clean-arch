namespace Architecture.Application.UseCases
{
    public record TestCommand
    {
        public string Name { get; set; }
        public string Family { get; set; }
    }

    public class TestCommandHandler(TestCommand command)
    {
        public async Task<Result> Handle(TestCommand command, CancellationToken cancellationToken)
        {
            return await Task.Run(Result.Success);
        }
    }
}
