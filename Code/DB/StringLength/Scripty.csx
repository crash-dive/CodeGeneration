#r "../Data.DB/bin/Release/net462/Data.DB.dll"

Output.SetFilePath("Folder/File.cs");
Output.BuildAction = BuildAction.GenerateOnly;

foreach (var schema in db.Schemas)
{
    var tablesWithStringColumns = schema.Tables.Where(t => t.Columns.Any(c => c.TypeInfo.CLRType == "string"));
    if (tablesWithStringColumns.Any())
    {
        Output.WriteLine($"namespace Data.StringLength.{schema.Name}");
        Output.WriteLine("{")
        foreach (var table in tablesWithStringColumns)
        {
            Output.WriteLine($"public static class {table.Name}StringLength");
            using (new Brace(Output))
            {
                foreach (var column in table.Columns)
                {
                    if (column.TypeInfo.CLRType == "string")
                    {
                        Output.WriteLine($"public const int {column.Name} = {column.MaxLength};");
                    }
                }
            }
        }
        Output.WriteLine("}");
    }
}