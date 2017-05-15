<#@ template language="C#" debug="true" hostspecific="true" #>
<#@ assembly name="Microsoft.VisualStudio.Shell.Interop.8.0" #>
<#@ assembly name="System.Core" #>
<#@ assembly name="EnvDTE" #>
<#@ assembly name="EnvDTE80" #>
<#@ assembly name="VSLangProj" #>
<#@ import namespace="System.Linq" #>
<#@ import namespace="System.Collections.Generic" #>
<#@ import namespace="Microsoft.VisualStudio.Shell.Interop" #>
<#@ import namespace="EnvDTE" #>
<#@ import namespace="EnvDTE80" #>
<#@ import namespace="Microsoft.VisualStudio.TextTemplating" #>
<#@ output extension=".cs" #>
using FleetAssist.UI.Web.Common.T4;
using System.Web.Routing;

<#

var project = GetCurrentProject();

foreach (var area in GetAreas(project))
{
	WriteLine("namespace FleetAssist.UI.Web.Areas." + area.Name + ".Controllers");
	WriteLine("{");
	PushIndent("	");	 

	foreach (var controller in GetControllers(area))
    {
		WriteLine("public static class " + controller.Name.Replace("Controller", ""));
		WriteLine("{");
		PushIndent("	");

		var methods = GetMethods(controller);
		var zeroParameterMethods = new HashSet<string>(); //If a method has overloads then we need to check if we have already created a zero param method for it
		foreach	(var method in methods)
		{
			var parameters = GetParameters(method);

			if (parameters.Count == 0 && zeroParameterMethods.Contains(method.Name) == false)
            {
				zeroParameterMethods.Add(method.Name);
				WriteMethodWithNoParameters(area, controller, method);
            }
			else
            {
				WriteMethodWithParameters(area, controller, method, parameters);

				//Ensures there is always a paramterless overload which can be called
				if (zeroParameterMethods.Contains(method.Name) == false)
				{
					zeroParameterMethods.Add(method.Name);
					WriteMethodWithNoParameters(area, controller, method);
				}
            }
		}

		PopIndent(); //Class
		WriteLine("}");
	}

	PopIndent(); //Namespace
	WriteLine("}");
}

#>

<#+
	Project GetCurrentProject()
	{
		var hostServiceProvider = (IServiceProvider)this.Host;
		var dte = (DTE)hostServiceProvider.GetService(typeof(DTE));
        return dte.Solution.OfType<Project>().First(p => p.Name == "UI.Web");
	}

	IEnumerable<ProjectItem> GetAreas(Project project)
	{
		return project.ProjectItems
			.Cast<ProjectItem>() //Cast for LINQ
			.First(f => f.Name == "Areas") //Get the root areas folder
			.ProjectItems.Cast<ProjectItem>() //Gets a list of areas
			.Where(a => a.Kind == EnvDTE.Constants.vsProjectItemKindPhysicalFolder); //Filter out non folder items like ViewStart in Areas
	}

	IEnumerable<CodeClass> GetControllers(ProjectItem area)
	{
		return area.ProjectItems
			.Cast<ProjectItem>() //Cast for LINQ
			.Where(a => a.Name == "Controllers") //Filters the subfolders down to just the controllers folder
			.SelectMany(c => c.ProjectItems.Cast<ProjectItem>()) //Gets all the controllers
			.SelectMany(c => c.FileCodeModel.CodeElements.Cast<CodeElement>()) //Gets the code elements in the controller file
			.Where(c => c.Kind == vsCMElement.vsCMElementNamespace) //Gets the only the namespaces
			.SelectMany(c => c.Children.Cast<CodeClass>()) //Converst the single class inside each namespace into CodeClass type
			.Where(c => c.IsAbstract == false); //We do not want any abstract classes
	}

	IEnumerable<CodeFunction> GetMethods(CodeClass controller)
	{
		return controller.Members
			.Cast<CodeElement>() //Cast for LINQ
			.Where(m => m.Kind == vsCMElement.vsCMElementFunction) //Gets all functions
			.Cast<CodeFunction>() //Converts to the function type
			.Where(f => f.Access == vsCMAccess.vsCMAccessPublic && f.FunctionKind == vsCMFunction.vsCMFunctionFunction); //Filters to public methods only, this removes the contructor and helper methods
	}

	IList<CodeParameter> GetParameters(CodeFunction method)
    {
		return method.Parameters
			.Cast<CodeParameter>()
			.ToList();
    }

	void WriteMethodWithNoParameters(ProjectItem area, CodeClass controller, CodeFunction method)
    {
		WriteLine("public static ActionRoute " + method.Name + "()");
		WriteLine("{");
		PushIndent("	");

		WriteLine("return new ActionRoute(\"" + controller.FullName + "\", \"" + method.Name + "\", \"" + controller.Name.Replace("Controller", "") + "\", \"" + area.Name + "\", new RouteValueDictionary());");

		PopIndent();
		WriteLine("}");
    }

	void WriteMethodWithParameters(ProjectItem area, CodeClass controller, CodeFunction method, IList<CodeParameter> parameters)
    {
		Write("public static ActionRoute " + method.Name + "(");
		for	(var i = 0; i < parameters.Count; i++)
		{
			var param = parameters[i];
			if (IsLastParameter(i, parameters)) //If it is the last param we do not want to add a comma to the end
			{
				Write(param.Type.AsFullName + " " + param.Name);
			}
			else
			{
				Write(param.Type.AsFullName + " " + param.Name + ", ");
			}
		}
		Write(")");
		WriteLine("");
		WriteLine("{");
		PushIndent("	");

		WriteLine("var routeValues = new RouteValueDictionary()");
		WriteLine("{");
		PushIndent("	");
		foreach	(var param in parameters)
		{
            if (param.Type.AsFullName == "System.DateTime") //We need these special cases because RouteValueDictionary does not respect the Culture of the DateTime and just uses the US
            {
                WriteLine("{ \"" + param.Name + "\", " + param.Name + ".ToString()},");
            }
            else if (param.Type.AsFullName == "System.Nullable<System.DateTime>")
            {
                WriteLine("{ \"" + param.Name + "\", " + param.Name + "?.ToString()},");
            }
            else
            {
                WriteLine("{ \"" + param.Name + "\", " + param.Name + "},");
            }
		}
		PopIndent();
		WriteLine("};");

		WriteLine("return new ActionRoute(\"" + controller.FullName + "\", \"" + method.Name + "\", \"" + controller.Name.Replace("Controller", "") + "\", \"" + area.Name + "\", routeValues);");

		PopIndent();
		WriteLine("}");
    }

	bool IsLastParameter(int i, IList<CodeParameter> parameters)
    {
		return i + 1 == parameters.Count;
    }
#>