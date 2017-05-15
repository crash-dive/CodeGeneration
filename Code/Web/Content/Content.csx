#r "System.IO"

using System.IO;

var templateDirectoryPath = Path.GetDirectoryName(ScriptFilePath);
var templateDirectory = new DirectoryInfo(Path.GetDirectoryName(ScriptFilePath));
var wwwrootDirectory = templateDirectory.Parent.Parent.GetDirectories("wwwroot")[0];
var wwwRootDirectoryUri = new Uri(wwwrootDirectory.FullName, UriKind.Absolute);

Output.SetFilePath(templateDirectoryPath + "\\Generated\\wwwroot.cs");
Output.BuildAction = BuildAction.GenerateOnly;

Output.WriteLine("");
Output.WriteLine("namespace FleetAssist.UI.Web.wwwroot");
Output.WriteLine("{");

ReadDirectories(wwwrootDirectory, wwwRootDirectoryUri);

Output.WriteLine("}");

var indentLevel = 0;
void ReadDirectories(DirectoryInfo parentDirectory, Uri wwwRootDirectoryUri)
{
    foreach (var directory in parentDirectory.EnumerateDirectories())
    {
        Output.Indent(++indentLevel);
        Output.WriteLine($"public static class {directory.Name}");
        Output.WriteLine("{");
        Output.Indent(++indentLevel);
        //Gets any other directories and creates them as sub static classes
        ReadDirectories(directory, wwwRootDirectoryUri);

        //Creates a const line for each file in the directory
        //Which will look like: public const string NameOfFile = @"PathToFile";
        foreach (var file in directory.EnumerateFiles())
        {
            Output.Write("public const string ");
            Output.Write(file.Name.Replace(".", "_").Replace("-", "_"));
            Output.Write(@" = """);
            Output.Write("/");
            Output.Write(wwwRootDirectoryUri.MakeRelativeUri(new Uri(file.FullName, UriKind.Absolute)).ToString()); //Make the paths start with \wwwroot\
            Output.Write(@""";");
            Output.WriteLine("");
        }
        Output.Indent(--indentLevel);
        Output.WriteLine("}");
        Output.Indent(--indentLevel);
    }
}