namespace crow.helpers;

public static class FSHelper
{
    public static void GotoDirectory(string path)
    {
        if(!Directory.Exists(path))
            Directory.CreateDirectory(path);
        Directory.SetCurrentDirectory(path);
    }

    public static void WriteToFile(string path,string text){
        using(StreamWriter sw = File.CreateText(path)){
            sw.WriteLine(text);
        }
    }

    public static string ReadFromFile(string path){
        using(StreamReader sr = new StreamReader(path)){
            return sr.ReadToEnd();
        }
    }

    public static void AppendFileToFile(string source_file, string destination_file){
        using(StreamWriter sw = File.AppendText(destination_file)){
            using(StreamReader sr = new StreamReader(source_file)){
                sw.WriteLine(sr.ReadToEnd());
            }

        }
    }

    public static void AppendTextToFile(string file, string text){
        using(StreamWriter sw = File.AppendText(file)){
            sw.WriteLine(text);
        }
    }

}