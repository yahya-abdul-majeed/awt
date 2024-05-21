namespace crow.models;

public class Metadata {
    public float time {get;set;}
    public float time_wall {get;set;}
    public int max_rss {get;set;}
    public int memory {get;set;}
    public int csw_voluntary {get;set;}
    public int csw_forced {get;set;}
    public int exitcode{get;set;}
    public string message {get;set;} = string.Empty;
    public string status {get;set;} = string.Empty;
}