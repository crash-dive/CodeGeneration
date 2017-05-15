<#@ template  debug="true" hostSpecific="true" #>
<#@ output extension=".cs" #>
<#@ assembly Name="System.Core" #>
<#@ assembly name="System.Data" #>
<#@ assembly name="System.Xml" #>
<#@ assembly name="Microsoft.VisualStudio.Shell.Interop.8.0" #>
<#@ assembly name="EnvDTE" #>
<#@ assembly name="EnvDTE80" #>
<#@ assembly name="VSLangProj" #>
<#@ assembly name="$(SolutionDir)DB.Metadata\bin\Debug\DB.Metadata.dll" #>
<#@ import namespace="DB.Metadata" #>
<#@ import namespace="DB.Metadata.DatabaseObject" #>
<#@ import namespace="System" #>
<#@ import namespace="System.IO" #>
<#@ import namespace="System.Diagnostics" #>
<#@ import namespace="System.Linq" #>
<#@ import namespace="System.Collections" #>
<#@ import namespace="System.Collections.Generic" #> 
<#@ include file="..\..\Common\Brace.ttinclude" #><##>

<#
    //This creates a list of all the string lengths in the database so we can use them when referancing them for validation
    var dbMetadata = new DBMetadata();

    foreach (var schema in dbMetadata.Schemas)
    {
        if (schema.Tables.Any())
        {
            WriteLine("namespace Data.StringLength." + schema.Name);
            using (new Brace(this))
            {
                foreach (var table in schema.Tables)
                {
                    WriteLine("public static class " + table.Name + "StringLength");
                    using (new Brace(this))
                    {
                        foreach (var column in table.Columns)
                        {
                            if (column.TypeInfo.CLRType == "string")
                            {
                                WriteLine("public const int " + column.Name + " = " + column.MaxLength + ";");
                            }
                        }
                    }
                    WriteLine("");
                }
            }
            WriteLine("");
        }
    }
#>