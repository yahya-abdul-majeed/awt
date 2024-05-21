using System.Text;
using crow.commands;
using crow.helpers;
using crow.models;
using crow.services;
using static crow.helpers.FSHelper;

namespace crow.src.jobs;

public class IsolateJob
{
    string EXECUTION_FILE;
    string CMD;
    long ELAPSED_TIME;
    string FINAL_FILE;
    const string STDIN_FILE_NAME = "stdin.txt";
    const string STDOUT_FILE_NAME = "stdout.txt";
    const string STDERR_FILE_NAME = "stderr.txt";
    const string METADATA_FILE_NAME = "metadata.txt";
    string FINAL_FILE_DIR;
    private int box_id; 
    private string workdir, boxdir, source_file, stdin_file,
    stdout_file, stderr_file, metadata_file;

    private ICommand Executor;
    private List<bool> resultList;
    private Submission? submission;
    private SubmissionContext context;
    private readonly ISubmissionService _ss;
    private readonly ICommand python3_unittest, dotnet_nunit3; 
    private Dictionary<string, ICommand> _map;
    public IsolateJob(ISubmissionService ss)
    {
        resultList = new();
        _ss = ss;
        submission = new();
        python3_unittest = new Python3_unittest();
        dotnet_nunit3 = new Dotnet_NUnit3();
        _map = new()
        {
            { "python3_unittest", python3_unittest },
            { "dotnet_nunit3", dotnet_nunit3 }
        };
    }

    public async Task<TestResult> Perform(int submission_id){
        try{
            submission = await _ss.GetSubmission(submission_id);
            context = await _ss.GetContext(submission.assignment_parent_id,submission.test_fw_id);
            Executor = _map[submission.lang_fw];
            await _ss.OnSubmissionStart(submission_id);

            var watch = System.Diagnostics.Stopwatch.StartNew();
            for (int i =0; i< context.Websites.Count; i++){
                Executor.GetInfo(context.Websites[i].url,out EXECUTION_FILE, out CMD); 
                Initialize_workdir(); 
                if(i==0)
                    Initialize_final_file();
                GotoDirectory(boxdir);
                Executor.PrepareForExec(submission.packages);

                run();
                resultList.Add(Executor.DidTestPass(context.Websites[i].should_pass));
                verify(i);

                Cleanup();

            }
            watch.Stop();
            ELAPSED_TIME = watch.ElapsedMilliseconds;


            TestResult result = new (){
                passed = resultList.All(b => b) || false ,
                result_file = File.ReadAllBytes(FINAL_FILE)
            };
            _ss.OnSubmissionEnd(submission_id,result); // use ELAPSED_TIME here
            GotoDirectory("/home");
            delete_final_file();
            return result;
        }
        catch(Exception ex){
            return new TestResult{
                passed = false ,
                result_file = Encoding.ASCII.GetBytes(ex.Message)
            };

        }

    }

    private void verify(int iteration)
    {
        AppendTextToFile("Elapsed time in ms: " + ELAPSED_TIME.ToString(),FINAL_FILE);
        AppendTextToFile(stdout_file,$"--------------------------------------\n"+
        $"Standard output for {context.Websites[iteration].url}, run: {iteration}" + 
        $"--------------------------------------\n");
        AppendFileToFile(stdout_file,FINAL_FILE);

        AppendTextToFile(stderr_file,$"Standard error for {context.Websites[iteration].url}, run: {iteration}");
        AppendFileToFile(stderr_file,FINAL_FILE);

        AppendTextToFile(metadata_file,$"metadata for {context.Websites[iteration].url}, run: {iteration}");
        AppendFileToFile(metadata_file,FINAL_FILE);
    }

    private void run()
    {
        try{
            // var mounts = "--dir etc --dir run/systemd/resolve --dir /home/yahya/.nuget/packages/nunit.consolerunner/3.16.3/tools "+
            // "--dir /home/yahya/.local/lib/python3.10/site-packages";
            var mounts = "--dir etc "+
            "--dir usr/local/lib/python3.9/dist-packages";
            var command = $"isolate --cg -b {box_id} -e --share-net {mounts} " +
            $"--meta {METADATA_FILE_NAME} --stdout {STDOUT_FILE_NAME} --stderr {STDERR_FILE_NAME} "+
            $"--run -- /bin/bash -c \"{CMD}\"";

            var res = command.BashExecute(workdir:boxdir,waitForExit:false).GetAwaiter().GetResult();

        }catch(Exception){
            //probably set a flag here for further processing.
        }
    }

    private void Initialize_file(string filename){
        _ = $"sudo touch {filename} && sudo chmod 666 {filename}".BashExecute(waitForExit:false,workdir:null).GetAwaiter().GetResult();
    }
    private void Initialize_final_file(){
       FINAL_FILE_DIR = $"/home/yahya/isolateResults/{box_id}/";
       FINAL_FILE = FINAL_FILE_DIR + $"final_file.txt";
       _ = $"sudo mkdir -p {FINAL_FILE_DIR}".BashExecute(workdir:workdir,waitForExit:true).GetAwaiter().GetResult();
       Initialize_file(FINAL_FILE);
    }
    private void delete_final_file(){
        _ = $"sudo rm -rf {FINAL_FILE_DIR}".BashExecute(workdir:null,waitForExit:true).GetAwaiter().GetResult();
    }

    private void Initialize_workdir(){
       GotoDirectory("/home");
       box_id = submission.id;
       workdir = $"isolate --cg -b {box_id} --init".BashExecute(workdir:"/var/local/lib/isolate",waitForExit:true).GetAwaiter().GetResult();
       workdir = workdir.Remove(workdir.Length -1);
       boxdir = workdir + "/box"; 
       source_file = boxdir + "/" + EXECUTION_FILE;
       stdin_file = boxdir + "/" + STDIN_FILE_NAME;
       stdout_file = boxdir + "/" +STDOUT_FILE_NAME;
       stderr_file = boxdir + "/" + STDERR_FILE_NAME;
       metadata_file = boxdir + "/" + METADATA_FILE_NAME;


       string[] files = {stdin_file,stdout_file,stderr_file,metadata_file};
       foreach(string f in files){
            Initialize_file(f);
       } 

       string modified_code = Executor.modify_code(submission.source_code,context.code_skeleton); 
       WriteToFile(source_file,modified_code);
    }

    private void Cleanup(){
        _ = $"isolate --cg -b {box_id} --cleanup".BashExecute(workdir:null,waitForExit:false).GetAwaiter().GetResult();
    }
}

