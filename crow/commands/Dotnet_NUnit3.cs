using crow.helpers;
using crow.models;
using System.Xml;

namespace crow.commands;

internal class Dotnet_NUnit3 : ICommand
{
    private const string EXECUTIONFILE = "UnitTest1.cs";
    const string TESTRESULTXML_FILE = "TestResult.xml";
    private const string PROJECT_FILE = "TestProject.csproj";
    private const string DLL = "TestProject1.dll";
    private const string DLL_PATH= $"bin/Debug/net6.0/{DLL}";
    private void AddProjectFilesToSandbox()
    {
        "sudo cp ~/dotnettests/* .".BashExecute(workdir:null,waitForExit:true).GetAwaiter().GetResult();
    }

    private void Compile(){
        var res = "dotnet build".BashExecute(workdir:null,waitForExit:true).GetAwaiter().GetResult();
    }

    public void GetInfo(string url,out string EXECUTION_FILE, out string CMD)
    {
        string options = $"--testparam:url={url}";
        EXECUTION_FILE = EXECUTIONFILE;
        CMD = $"nunit3-console.exe {DLL_PATH} {options}";
    }

    public void PrepareForExec(string packages)
    {
        AddProjectFilesToSandbox();
        AddPackages(packages);
        Compile();
    }

    private void AddPackages(string packages)
    {
        if(!string.IsNullOrEmpty(packages)){
            string[] arr = packages.Split(',');
            foreach(string package in arr){
                $"dotnet add package {package}".BashExecute(workdir:null,waitForExit:true).GetAwaiter().GetResult();
            }
        }
    }

    public string modify_code(string code, Skeleton skeleton)
    {
        int index;
        int startIndex = 0;

        do
        {
            index = code.IndexOf("using",startIndex);
            if (index != -1) startIndex += 5;

        } while (index != -1);

        int insertIndex = code.IndexOf("\n", startIndex) + 1 ;
        return code.Insert(insertIndex,skeleton.code_before);
    }

    public bool DidTestPass(bool should_pass)
    {
        //gotta compare it to expected result before deciding
        using(XmlReader reader = XmlReader.Create(TESTRESULTXML_FILE)){
            while(reader.Read()){
                if(reader.NodeType == XmlNodeType.Element && reader.Name == "test-run"){
                    string? result = reader.GetAttribute("result");
                    if(result == "Passed") 
                        return true;
                    else 
                        return false;
                }
            }
            return false;
        }
    }
}