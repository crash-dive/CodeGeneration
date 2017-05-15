<#@ template  debug="true" hostSpecific="true" #>
<#@ output extension=".cs" #>
<#@ assembly Name="System.Core" #>
<#@ assembly name="System.Data" #>
<#@ assembly name="System.Xml" #>
<#@ assembly name="Microsoft.VisualStudio.Shell.Interop.8.0" #>
<#@ assembly name="EnvDTE" #>
<#@ assembly name="EnvDTE80" #>
<#@ assembly name="VSLangProj" #>
<#@ assembly name="$(SolutionDir)DB.Atlas.Metadata\bin\Debug\FleetAssist.DB.Atlas.Metadata.dll" #>
<#@ import namespace="FleetAssist.DB.Atlas.Metadata" #>
<#@ import namespace="FleetAssist.DB.Atlas.Metadata.DatabaseObject" #>
<#@ import namespace="FleetAssist.DB.Atlas.Metadata.DatabaseObject.Procedure" #>
<#@ import namespace="FleetAssist.DB.Atlas.Metadata.DatabaseObject.Procedure.Column" #>
<#@ import namespace="FleetAssist.DB.Atlas.Metadata.DatabaseObject.Procedure.Parameter" #>
<#@ import namespace="FleetAssist.DB.Atlas.Metadata.DatabaseObject.UserDefinedType" #>
<#@ import namespace="FleetAssist.DB.Atlas.Metadata.DatabaseObject.UserDefinedType.Column" #>
<#@ import namespace="FleetAssist.DB.Atlas.Metadata.TypeMapping" #>
<#@ import namespace="System" #>
<#@ import namespace="System.IO" #>
<#@ import namespace="System.Diagnostics" #>
<#@ import namespace="System.Linq" #>
<#@ import namespace="System.Collections" #>
<#@ import namespace="System.Collections.Generic" #>
<#@ import namespace="System.Data" #>
<#@ import namespace="System.Data.SqlClient" #>
<#@ import namespace="System.Text" #>
<#@ import namespace="System.Xml" #>
<#@ import namespace="EnvDTE" #>
<#@ import namespace="EnvDTE80" #>
<#@ include file="..\..\Common\Brace.ttinclude" #><##>
<#@ include file="Writer/Class/ClassWriterBase.ttinclude" #><##>
<#@ include file="Writer/Class/ResultClass.ttinclude" #><##>
<#@ include file="Writer/Class/UserDefinedTypeClass.ttinclude" #><##>
<#@ include file="Writer/Method/MethodWriterBase.ttinclude" #><##>
<#@ include file="Writer/Method/NonQuery.ttinclude" #><##>
<#@ include file="Writer/Method/ReadIntoExistingCustomTypeList.ttinclude" #><##>
<#@ include file="Writer/Method/ReadIntoNewCustomTypeList.ttinclude" #><##>
<#@ include file="Writer/Method/ReadIntoReturnTypeList.ttinclude" #><##>
<#@ include file="Writer/Method/XmlQuery.ttinclude" #><##>
<#@ include file="Writer/Method/ScalarQuery.ttinclude" #><##>
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using FleetAssist.Data.Attribute;
<#
    //Get all the meta data about the database
    var atlasDBMetadata = new AtlasDBMetadata();

    foreach (var namespaceName in atlasDBMetadata.EnumMap.EnumNamespaces)
    {
        WriteLine("using " + namespaceName + ";");
    }
#>

<#
    //Create a facade class to pass referances to the stored procedures around
    //This is to stop massive constructor bloat
    WriteLine("namespace FleetAssist.Data.SP");
    using (new Brace(this))
    {
        WriteLine("public class StoredProcs");
        using (new Brace(this))
        {
            foreach (var schema in atlasDBMetadata.Schemas)
            {
                if (schema.Procedures.Any())
                {
                    WriteLine("public Schema." + schema.Name + " " + schema.Name + "  { get; } = new Schema." + schema.Name + "();");
                }
            }
        }
    }
#>

<#
    //Method Writers: These have a Write method which render the method signature used to read that type of proc
    var nonQuery = new NonQuery(this);
    var xmlQuery = new XmlQuery(this);
    var scalarQuery = new ScalarQuery(this);
    var readIntoReturnTypeList = new ReadIntoReturnTypeList(this);
    var readIntoNewCustomTypeList = new ReadIntoNewCustomTypeList(this);
    var readIntoExistingCustomTypeList = new ReadIntoExistingCustomTypeList(this);

    //Class Writers: These have a Write method which renders a class used by the proc to either return data or read it
    var resultClass = new ResultClass(this);
    var userDefinedTypeClass = new UserDefinedTypeClass(this);

    //Create a class for each schema with each stored procedures implemented as a method on those classes
    WriteLine("namespace FleetAssist.Data.SP.Schema");
    using (new Brace(this))
    {
        foreach (var schema in atlasDBMetadata.Schemas)
        {
            if (schema.Procedures.Any() || schema.UserDefinedTypes.Any())
            {
                WriteLine("public class " + schema.Name);
                using (new Brace(this))
                {
                    foreach (var udt in schema.UserDefinedTypes)
                    {
                        userDefinedTypeClass.Write(udt);

                        WriteLine("");
                    }

                    //Write out each method used to call the procedure
                    foreach (var proc in schema.Procedures)
                    {
                        if (proc.Columns.Any() == false) //Does not return anything
                        {
                            nonQuery.Write(proc);
                        }
                        else if (proc.IsXml) //Returns an XmlReader
                        {
                            xmlQuery.Write(proc);
                        }
                        else if (proc.IsScalar) //Returns a Single Value
                        {
                            scalarQuery.Write(proc);
                        }
                        else //Returns a list of result classes
                        {
                            readIntoReturnTypeList.Write(proc);

                            WriteLine("");

                            readIntoNewCustomTypeList.Write(proc);

                            WriteLine("");

                            readIntoExistingCustomTypeList.Write(proc);

                            WriteLine("");

                            resultClass.Write(proc);
                        }

                        WriteLine("");
                    }
                }

                WriteLine("");
            }
        }
    }    
#>