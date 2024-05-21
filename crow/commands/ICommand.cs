using crow.models;

namespace crow.commands;

internal interface ICommand{
    void GetInfo(string url,out string EXECUTION_FILE, out string CMD);
    void PrepareForExec(string packages);
    string modify_code(string code, Skeleton skeleton);
    bool DidTestPass(bool should_pass);

}