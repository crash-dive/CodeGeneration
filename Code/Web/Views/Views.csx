#r "System.IO"

using System.IO;

var templateDirectoryPath = Path.GetDirectoryName(ScriptFilePath);
var templateDirectory = new DirectoryInfo(templateDirectoryPath);
var projectDirectory = templateDirectory.Parent.Parent;

Output.SetFilePath(templateDirectoryPath + "\\Generated\\Views.cs");
Output.BuildAction = BuildAction.GenerateOnly;

Output.WriteLine("using System;");
Output.WriteLine("using System.Collections.Generic;");
Output.WriteLine("using FleetAssist.UI.Web.Common.View;");
Output.WriteLine("");

var areasDirectory = projectDirectory.GetDirectories("Areas")[0];
foreach (var area in areasDirectory.EnumerateDirectories())
{
    ReadArea(area, projectDirectory);
}

var commonDirectory = projectDirectory.GetDirectories("Common")[0];
foreach (var commonSubDirectory in commonDirectory.EnumerateDirectories())
{
    foreach (var file in commonSubDirectory.GetFiles())
    {
        if (file.Extension == ".cshtml")
        {
            ReadCommonFolder(commonSubDirectory, projectDirectory);
            break;
        }
    }
}

var indentLevelArea = 0;
void ReadArea(DirectoryInfo area, DirectoryInfo rootDirectory)
{
    var views = area.GetDirectories("Views");
    if (views.Length == 1)
    {
        Output.WriteLine($"namespace FleetAssist.UI.Web.Areas.{area.Name}");
        Output.WriteLine("{");
        Output.Indent(++indentLevelArea);

        Output.WriteLine("public static class Views");
        Output.WriteLine("{");
        Output.Indent(++indentLevelArea);

        ReadFiles(views[0], rootDirectory);
        ReadDirectories(views[0], rootDirectory);

        Output.Indent(--indentLevelArea);
        Output.WriteLine("}");

        Output.Indent(--indentLevelArea);
        Output.WriteLine("}");
    }
}

var indentLevelCommonFolder = 0;
void ReadCommonFolder(DirectoryInfo commonSubDirectory, DirectoryInfo rootDirectory)
{
        Output.WriteLine($"namespace FleetAssist.UI.Web.Common.{commonSubDirectory.Name}");
        Output.WriteLine("{");
        Output.Indent(++indentLevelCommonFolder);

        Output.WriteLine("public static class Views");
        Output.WriteLine("{");
        Output.Indent(++indentLevelCommonFolder);

        ReadFiles(commonSubDirectory, rootDirectory);
        ReadDirectories(commonSubDirectory, rootDirectory);

        Output.Indent(--indentLevelCommonFolder);
        Output.WriteLine("}");

        Output.Indent(--indentLevelCommonFolder);
        Output.WriteLine("}");
}

var indentLevelDirectories = 0;
void ReadDirectories(DirectoryInfo parentDirectory, DirectoryInfo rootDirectory)
{
    Output.Indent(++indentLevelDirectories);
    foreach (var directory in parentDirectory.EnumerateDirectories())
    {
        Output.Indent(++indentLevelDirectories);
        Output.WriteLine("public static class " + directory.Name);
        Output.WriteLine("{");
        Output.Indent(++indentLevelDirectories);

        //Gets any other directories and creates them as sub static classes
        ReadDirectories(directory, rootDirectory);

        ReadFiles(directory, rootDirectory);

        Output.Indent(--indentLevelDirectories);
        Output.WriteLine("}");
        Output.Indent(--indentLevelDirectories);
    }
    Output.Indent(--indentLevelDirectories);

}

/// <summary>
/// Creates a const line for each file in the directory.
/// Which will look like: public const string NameOfFile = @"PathToFile";
/// </summary>
void ReadFiles(DirectoryInfo directory, DirectoryInfo rootDirectory)
{
    foreach (var file in directory.EnumerateFiles())
    {
        if (file.Extension == ".cshtml")
        {
            var viewlocation = GetViewLocationType(file);
            var propertyName = GetPropertyName(directory, file);
            var filePath = file.FullName.Replace(rootDirectory.FullName, "");

            Output.Write(
                $"public static {viewlocation} {propertyName} {{ get; }} = new {viewlocation}(@\"~{filePath}\");"
            );

            Output.WriteLine("");
        }
    }
}

string GetViewLocationType(FileInfo file)
{
    //Open the stream and read it back.
    using (var stream = file.OpenText())
    {
        var line = String.Empty;
        var lineCount = 0;
        while ((line = stream.ReadLine()) != null)
        {
            if (line.StartsWith("@model "))
            {
                return $"ViewLocation<{line.Replace("@model ", "")}>";
            }
            else if (lineCount > 5)
            {
                return "ViewLocation";
            }
            else
            {
                lineCount++;
            }
        }
    }

    return "ViewLocation";
}

string GetPropertyName(DirectoryInfo rootDirectory, FileInfo file)
{
    var fileName = Path.GetFileNameWithoutExtension(file.Name).Replace(".", "_").Replace("-", "_");

    if (rootDirectory.Name == fileName)
    {
        return fileName + "View"; //A member cannot have the same name as its containing type
    }
    else
    {
        return fileName;
    }
}
