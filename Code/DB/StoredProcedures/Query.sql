--Procs
SELECT 
	schemas.schema_id,
	schemas.name,
	procedures.object_id,
	procedures.name
FROM 
	sys.procedures

	INNER JOIN sys.schemas
		ON schemas.schema_id = procedures.schema_id
ORDER BY
	schemas.name,
	procedures.name;
	
--Columns
SELECT
	procedures.object_id,
	columns.name,
	(columns.column_ordinal - 1) AS ColumnOrdinal,
	columns.is_nullable,
	types.name
FROM 
	sys.procedures

	CROSS APPLY sys.dm_exec_describe_first_result_set_for_object(procedures.object_id, NULL) AS columns

	INNER JOIN sys.types
		ON types.user_type_id = ISNULL(columns.user_type_id, columns.system_type_id);
		
		
--Params
SELECT 
	procedures.object_id,
	parameters.name,
	parameters.max_length,
	parameters.is_output,
	types.name
FROM   
	sys.procedures

	INNER JOIN sys.parameters
			ON parameters.object_id = procedures.object_id

	INNER JOIN sys.types
		ON types.user_type_id = parameters.user_type_id
ORDER BY 
	procedures.object_id,
	parameters.parameter_id