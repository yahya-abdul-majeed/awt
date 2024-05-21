using crow.models;
using crow.helpers;
using System.Text.Json;

namespace crow.commands;

internal class Python3_unittest : ICommand
{
    private const string EXECUTIONFILE = "code.py";
    private const string REQUIREMENTS_FILE = "requirements.txt";
    public bool DidTestPass(bool should_pass)
    {
        //add logic when stdout.txt cannot be successfully read
        try{
            var data = FSHelper.ReadFromFile("stdout.txt");
            var result = JsonSerializer.Deserialize<Python3UnittestResult>(data);
            return result.verdict == should_pass;
        }catch(Exception){
            return false;
        }
    }

    public void GetInfo(string url, out string EXECUTION_FILE, out string CMD)
    {
        EXECUTION_FILE = EXECUTIONFILE;
        CMD = $"python3 {EXECUTIONFILE} {url}";
    }

    public string modify_code(string code, Skeleton skeleton)
    {
       code += skeleton.code_after.Replace("\\r", "\r").Replace("\\n","\n").Replace("\\\"","\"");
       var modifiedCode = skeleton.code_before.Replace("\\r", "\r").Replace("\\n","\n").Replace("\\\"","\"") + code;
       return modifiedCode;
    }

    public void PrepareForExec(string packages)
    {
        AddPackages();
        AddProjectFilesToSandbox();
    }
    private void AddProjectFilesToSandbox(){
        _ = "cp -r ~/hitsunittest .".BashExecute(workdir:null,waitForExit:false).GetAwaiter().GetResult();
    }
    private void AddPackages(){
        _ = "pipreqs --force .".BashExecute(workdir:null,waitForExit:true).GetAwaiter().GetResult();
        _ = $"pip install -r {REQUIREMENTS_FILE}".BashExecute(workdir:null,waitForExit:true).GetAwaiter().GetResult();
    }
}


public class Python3UnittestResult{
    public List<string> failures {get;set;} = new();
    public List<string> errors {get;set;} = new();
    public int testsRun {get;set;}
    public bool verdict {get;set;}
    public string userOutput {get;set;} = string.Empty;
}