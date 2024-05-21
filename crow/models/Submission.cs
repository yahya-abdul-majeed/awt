namespace crow.models;

public class Submission
{
    public int id{get;}
    public int assignment_parent_id {get;}
    public int test_fw_id {get;}
    public string lang_fw {get;} = string.Empty;
    public string source_code {get;} = string.Empty;
    public string packages {get;} = string.Empty;

    public void OnStart(){//status, started_at, execution_host

    }
    public void OnFinish(){

    } 
}

public enum Status
{
    INQUEUE,
    PROCESSING,
    COMPLETED,
    COMPILATIONERROR,
    RE,
    SG,
    TO,
    XX
}
