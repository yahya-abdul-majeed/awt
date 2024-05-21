using System.Diagnostics;

namespace crow.helpers;

public static class ShellHelper{
    public static Task<string> BashExecute(this string command, string? workdir, bool waitForExit){

        TaskCompletionSource<string> tcs = new();
        string escapedArgs = command.Replace("\"","\\\"");
        ProcessStartInfo psi = new ProcessStartInfo{
            FileName = "bash",
            Arguments = $"-c \"{escapedArgs}\"",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
            WorkingDirectory = !string.IsNullOrEmpty(workdir) ? workdir : Directory.GetCurrentDirectory()
        };
        Process process = new Process{
            StartInfo = psi,
            EnableRaisingEvents = true,
        };

        process.Exited += (sender, eventArgs) => {
            if(process.ExitCode == 0)
                tcs.SetResult(process.StandardOutput.ReadToEnd());
            else
                tcs.SetException(new Exception($"Command `{command}` failed with exit code `{process.ExitCode}`"));

            process.Dispose();
        };

        try{
            process.Start();
            if(waitForExit) process.WaitForExit();
        }
        catch(Exception ex){
            tcs.SetException(ex);
        }

        return tcs.Task;
    }
}