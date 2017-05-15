<#@ template debug="false" hostspecific="true" language="C#" #>
<#@ assembly name="System.Core" #>
<#@ import namespace="System.IO" #>
<#@ output extension=".cs" #>

namespace FleetAssist.UI.Web.wwwroot
{
<# 
	var templateDirectory = new DirectoryInfo(Path.GetDirectoryName(Host.TemplateFile));
	var wwwrootDirectory = templateDirectory.Parent.Parent.GetDirectories("wwwroot")[0];
	var wwwRootDirectoryUri = new Uri(wwwrootDirectory.FullName, UriKind.Absolute);
	ReadDirectories(wwwrootDirectory, wwwRootDirectoryUri);
#>
}
<#+
	void ReadDirectories(DirectoryInfo parentDirectory, Uri wwwRootDirectoryUri)
	{
        foreach (var directory in parentDirectory.EnumerateDirectories())
        {
			//Creates a static class for each directory
			PushIndent("	");
			WriteLine("public static class " + directory.Name); 
			WriteLine("{");
			PushIndent("	");

			//Gets any other directories and creates them as sub static classes
			ReadDirectories(directory, wwwRootDirectoryUri);

			//Creates a const line for each file in the directory
			//Which will look like: public const string NameOfFile = @"PathToFile";
            foreach (var file in directory.EnumerateFiles())
            {
				Write("public const string "); 
				Write(file.Name.Replace(".", "_").Replace("-", "_"));
				Write(@" = """);
				Write("/");
				Write(wwwRootDirectoryUri.MakeRelativeUri(new Uri(file.FullName, UriKind.Absolute)).ToString()); //Make the paths start with \wwwroot\
				Write(@""";");
				WriteLine("");
            }
			PopIndent();
			WriteLine("}");
			PopIndent();
        }
	}
#>