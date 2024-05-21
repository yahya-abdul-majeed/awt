namespace lynx.Models
{
    public class AggregateResult
    {
        public bool passed { get; set; } = true;
        public List<ExecutionResult> executionResults { get; set; } = new();
    }
}
