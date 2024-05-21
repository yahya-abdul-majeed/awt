namespace crow.models;

public class BashResult
{
    public BashResult(string standardError, string standardOutput)
    {
       this.StandardError = standardError;
       this.StandardOutput = standardOutput; 
    }
    public string StandardError {get;set;} = string.Empty;
    public string StandardOutput {get;set;} = string.Empty;
}