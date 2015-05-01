SELECT *
FROM sys.system_columns
order by name

SELECT *
FROM INFORMATION_SCHEMA.TABLES

SELECT *
FROM INFORMATION_SCHEMA.COLUMNS
order by TABLE_CATALOG, TABLE_SCHEMA, TABLE_NAME, ORDINAL_POSITION

select top 100 * from sys.objects so 
where so.type_desc <> 'DEFAULT_CONSTRAINT'
AND   so.type_desc <> 'USER_TABLE'
AND   so.type_desc <> 'SQL_TRIGGER'
AND   so.type_desc <> 'SQL_STORED_PROCEDURE'
AND   so.type_desc <> 'INTERNAL_TABLE'
AND   so.type_desc <> 'PRIMARY_KEY_CONSTRAINT'
AND   so.type_desc <> 'SQL_SCALAR_FUNCTION'
AND   so.type_desc <> 'FOREIGN_KEY_CONSTRAINT'
--AND   so.type_desc <> 'VIEW'
AND   so.type_desc <> 'CHECK_CONSTRAINT'
AND   so.type_desc <> 'TYPE_TABLE'
AND   so.type_desc <> 'SQL_TABLE_VALUED_FUNCTION'
AND   so.type_desc <> 'SERVICE_QUEUE'
and	  SO.name NOT LIKE '%FULLTEXT%'
order by so.name


SELECT *
FROM SYS.tables

select *
from sys.system_columns as sc
join sys.objects so on sc.collation_name = so.object_id

select *
from sys.types


SELECT distinct TABLE_TYPE
FROM INFORMATION_SCHEMA.TABLES

SELECT top 1000 * FROM INFORMATION_SCHEMA.TABLES order by newid()