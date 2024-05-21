namespace crow.models;

public class SubmissionContext {
    public List<ContextItem> Websites { get;set; } = new();
    public Skeleton code_skeleton {get;set;} = new();
}

public class Skeleton {
    public string code_before{get;set;} = string.Empty;
    public string code_after{get;set;} = string.Empty;
}

public class ContextItem
{
    public string url { get;set; } = string.Empty;
    public bool should_pass {get;set;} = default; 
}
