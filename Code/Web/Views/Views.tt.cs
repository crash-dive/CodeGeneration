<#@ template debug="false" hostspecific="true" language="C#" #>
<#@ assembly name="System.Core" #>
<#@ import namespace="System.IO" #>
<#@ import namespace="System.Linq" #>
<#@ output extension=".cs" #>
using System;
using System.Collections.Generic;
using FleetAssist.UI.Web.Common.View;

<# 
	var templateDirectory = new DirectoryInfo(Path.GetDirectoryName(Host.TemplateFile));
	var rootDirectory = templateDirectory.Parent.Parent;

	var areasDirectory = rootDirectory.GetDirectories("Areas")[0];
	foreach (var area in areasDirectory.EnumerateDirectories())
	{
		ReadArea(area, rootDirectory);
	}

	var commonDirectory = rootDirectory.GetDirectories("Common")[0];
	foreach (var commonSubDirectory in commonDirectory.EnumerateDirectories())
	{
		ReadCommonFolder(commonSubDirectory, rootDirectory);
	}
#>

<#+
	void ReadArea(DirectoryInfo area, DirectoryInfo rootDirectory)
	{
		var views = area.GetDirectories("Views");
        if (views.Length == 1)
        {
			WriteLine("namespace FleetAssist.UI.Web.Areas." + area.Name);
			WriteLine("{");
			PushIndent("	");

				WriteLine("public static class Views"); 
				WriteLine("{");
				PushIndent("	");

				ReadFiles(views[0], rootDirectory);
				ReadDirectories(views[0], rootDirectory);

				PopIndent();
				WriteLine("}");

			PopIndent();
			WriteLine("}");
        }
	}

	void ReadCommonFolder(DirectoryInfo commonSubDirectory, DirectoryInfo rootDirectory)
	{
        if (commonSubDirectory.EnumerateFiles("*.cshtml").Any())
        {
			WriteLine("namespace FleetAssist.UI.Web.Common." + commonSubDirectory.Name);
			WriteLine("{");
			PushIndent("	");

				WriteLine("public static class Views");
				WriteLine("{");
				PushIndent("	");

				ReadFiles(commonSubDirectory, rootDirectory);
				ReadDirectories(commonSubDirectory, rootDirectory);

				PopIndent();
				WriteLine("}");

			PopIndent();
			WriteLine("}");
        }
	}

	void ReadDirectories(DirectoryInfo parentDirectory, DirectoryInfo rootDirectory)
	{
        foreach (var directory in parentDirectory.EnumerateDirectories())
        {
			//Creates a static class for each directory			
			WriteLine("public static class " + directory.Name); 
			WriteLine("{");
			PushIndent("	");

			//Gets any other directories and creates them as sub static classes
			ReadDirectories(directory, rootDirectory);

			//Adds the file consts
			ReadFiles(directory, rootDirectory);

			PopIndent();
			WriteLine("}");
        }
	}

	void ReadFiles(DirectoryInfo directory, DirectoryInfo rootDirectory)
	{
		//Creates a const line for each file in the directory
		//Which will look like: public const string NameOfFile = @"PathToFile";
		foreach (var file in directory.EnumerateFiles())
		{
			if (file.Extension == ".cshtml")
            {
				var viewlocation = GetViewLocationType(file);
				var propertyName = GetPropertyName(directory, file);
				var filePath = file.FullName.Replace(rootDirectory.FullName, "");

				Write(
					"public static " +  viewlocation + " " + propertyName + " { get; } = new " + viewlocation + "(@\"~" + filePath + "\");"
				);

				WriteLine("");
            }
		}
	}

	string GetViewLocationType(FileInfo file)
	{
	    // Open the stream and read it back.
        using (var stream = file.OpenText())
        {
            var line = String.Empty;
			var lineCount = 0;
            while ((line = stream.ReadLine()) != null) 
            {
                if (line.StartsWith("@model "))
				{
					return "ViewLocation<" + line.Replace("@model ", "") + ">";
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
#>