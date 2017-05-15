using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

var templateDirectoryPath = Path.GetDirectoryName(ScriptFilePath);
Output.SetFilePath(templateDirectoryPath + "\\Generated\\Actions.cs");
Output.BuildAction = BuildAction.GenerateOnly;

Output.WriteLine("using System;");
Output.WriteLine("using System.Web.Routing;");
Output.WriteLine("using FleetAssist.UI.Web.Common.Action;");
Output.WriteLine("");

int indentLevel = 0;
foreach (var document in Project.Analysis.Documents)
{
    if (document.Name.EndsWith("Controller.cs"))
    {
        var tree = document.GetSyntaxTreeAsync().Result;
        var root = tree.GetRoot() as CompilationUnitSyntax;
        var semanticModel = document.GetSemanticModelAsync().Result;

        var namespaceSyntax = root.Members[0] as NamespaceDeclarationSyntax;
        var classSyntax = namespaceSyntax.Members[0] as ClassDeclarationSyntax;
        
        if (classSyntax.Modifiers.Any(m => m.IsKind(SyntaxKind.AbstractKeyword)) == false)
        {
            Output.WriteLine($"namespace {namespaceSyntax.Name.ToString()}");
            Output.WriteLine("{");
            Output.Indent(++indentLevel);

            Output.WriteLine($"public static class {classSyntax.Identifier.ToString().Replace("Controller", "")}");
            Output.WriteLine("{");
            Output.Indent(++indentLevel);

            //If a method has overloads then we need to check if we have already created a zero param method for it
            var zeroParameterMethods = new HashSet<string>();
            foreach (var member in classSyntax.Members)
            {
                //Methods but not constructors
                if (member is MethodDeclarationSyntax method && method.IsKind(SyntaxKind.ConstructorDeclaration) == false)
                {
                    foreach (var modifier in method.Modifiers)
                    {
                        //We only want public methods
                        if (modifier.IsKind(SyntaxKind.PublicKeyword))
                        {
                            //For methods with no parameters
                            if (method.ParameterList.Parameters.Count == 0 && zeroParameterMethods.Contains(method.Identifier.Text) == false)
                            {
                                zeroParameterMethods.Add(method.Identifier.Text);
                                PrintZeroParameterMethod(namespaceSyntax, classSyntax, method);
                            }
                            //For methods with parameters
                            else
                            {
                                PrintParameterMethod(semanticModel, namespaceSyntax, classSyntax, method);

                                if (zeroParameterMethods.Contains(method.Identifier.Text) == false)
                                {
                                    zeroParameterMethods.Add(method.Identifier.Text);
                                    PrintZeroParameterMethod(namespaceSyntax, classSyntax, method);
                                }
                            }
                        }
                    }
                }
            }

            Output.Indent(--indentLevel);
            Output.WriteLine("}");

            Output.Indent(--indentLevel);
            Output.WriteLine("}");
        }
    }
}

void PrintParameterMethod(SemanticModel semanticModel, NamespaceDeclarationSyntax namespaceSyntax, ClassDeclarationSyntax classSyntax, MethodDeclarationSyntax method)
{
    Output.Write($"public static ActionRoute {method.Identifier.Text}(");

    var lastParam = method.ParameterList.Parameters.Last();

    foreach (var param in method.ParameterList.Parameters)
    {
        var paramType = semanticModel.GetTypeInfo(param.Type).Type as INamedTypeSymbol;
        var paramTypeName = paramType.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);

        if (paramType.IsGenericType)
        {
            if (paramTypeName.EndsWith("?")) //Nullable types
            {
                var nullableType = paramType.TypeArguments.First();

                PrintParamterMethodArgumentType(
                    nullableType.ContainingNamespace.ToString(),
                    nullableType.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)
                );

                Output.Write("?");
                Output.Write($" {param.Identifier.Text}");
            }
            else //Fully generic types
            {
                PrintParamterMethodArgumentType(
                    paramType.ContainingNamespace.ToString(),
                    paramTypeName
                );

                Output.Write("<");

                var lastGenericParam = paramType.TypeArguments.Last();

                foreach (var genericParam in paramType.TypeArguments)
                {
                    PrintParamterMethodArgumentType(
                        genericParam.ContainingNamespace.ToString(),
                        genericParam.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)
                    );

                    if (genericParam != lastGenericParam)
                    {
                        Output.Write(", ");
                    }
                }

                Output.Write(">");
                Output.Write($" {param.Identifier.Text}");
            }
        }
        else //Standard types
        {
            PrintParamterMethodArgumentType(
                paramType.ContainingNamespace.ToString(),
                paramTypeName
            );

            Output.Write($" {param.Identifier.Text}");
        }

        if (param != lastParam)
        {
            Output.Write(", ");
        }
    }

    Output.WriteLine(")");
    Output.WriteLine("{");
    Output.Indent(++indentLevel);

    Output.WriteLine("var routeValues = new RouteValueDictionary()");
    Output.WriteLine("{");
    Output.Indent(++indentLevel);

    foreach (var parameter in method.ParameterList.Parameters)
    {
        Output.Write("{");
        Output.Write($@" ""{parameter.Identifier.Text}"", {parameter.Identifier.Text} ");

        if (parameter == lastParam)
        {
            Output.WriteLine("}");
        }
        else
        {
            Output.WriteLine("},");
        }
    }

    Output.Indent(--indentLevel);
    Output.WriteLine("};");

    PrintActionRouteCtor(namespaceSyntax, classSyntax, method, isZeroParams: false);

    Output.Indent(--indentLevel);
    Output.WriteLine("}");
}

void PrintParamterMethodArgumentType(string paramNamespace, string paramTypeName)
{
    if (paramNamespace != "System") //We have a using for this so do not need to print it
    {
        Output.Write($"{paramNamespace}.{paramTypeName}");
    }
    else
    {
        Output.Write(paramTypeName);
    }
}

void PrintZeroParameterMethod(NamespaceDeclarationSyntax namespaceSyntax, ClassDeclarationSyntax classSyntax, MethodDeclarationSyntax method)
{
    Output.WriteLine($"public static ActionRoute {method.Identifier.Text}()");
    Output.WriteLine("{");
    Output.Indent(++indentLevel);

    PrintActionRouteCtor(namespaceSyntax, classSyntax, method, isZeroParams: true);

    Output.Indent(--indentLevel);
    Output.WriteLine("}");
}

void PrintActionRouteCtor(NamespaceDeclarationSyntax namespaceSyntax, ClassDeclarationSyntax classSyntax, MethodDeclarationSyntax method, bool isZeroParams)
{
    Output.Write("return new ActionRoute(");

    //Full type name
    Output.Write($@"""{namespaceSyntax.Name.ToString()}.{classSyntax.Identifier.ToString()}"", ");

    //Action
    Output.Write($@"""{method.Identifier.Text}"", ");

    //Controller
    Output.Write($@"""{classSyntax.Identifier.ToString().Replace("Controller", "")}"", ");

    //Area
    var area = namespaceSyntax.Name
                              .ToString()
                              .Split(new string[] { "Areas.", ".Controllers" }, StringSplitOptions.None)
                              [1];

    Output.Write($@"""{area}"", ");

    //RouteValue
    if (isZeroParams)
    {
        Output.Write("new RouteValueDictionary()");
    }
    else
    {
        Output.Write("routeValues");
    }

    Output.WriteLine(");");
}
