namespace Architecture.Domain.Abstractions;

public sealed class FakeCurrentOperator : IOperatorRequester
{
    public Guid OperatorAccountId => new("019B69D7-6959-707C-9C5A-E611A9D3E48C");
}
