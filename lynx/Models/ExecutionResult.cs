namespace lynx.Models;

public class ExecutionResult
{
    public string PackageInstallationStandardOutput {get;set;} = string.Empty;
    public string PackageInstallationStandardError {get;set;} = string.Empty;
    public string CompilationStandardOutput {get;set;} = string.Empty;
    public string CompilationStandardError {get;set;} = string.Empty;
    public string RunStandardOutput {get;set;} = string.Empty;
    public string RunStandardError {get;set;} = string.Empty;
    public TestResult testResult { get; set; } = new();
}

//public class TestResult
//{

    //public float timeTaken { get; set; }
    //public List<string> failures { get; set; } = new();
    //public List<string> errors { get; set; } = new();
    //public int testsRun { get; set; }
    //public bool verdict { get; set; }
    //public string userOutput { get; set; } = string.Empty;
//}
public class TestResult
{
    public bool passed { get; set; }
    public byte[] result_file { get; set; }
}