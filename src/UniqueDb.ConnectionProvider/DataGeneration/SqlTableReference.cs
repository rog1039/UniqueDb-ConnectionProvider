using System;
using FluentAssertions;

namespace UniqueDb.ConnectionProvider.DataGeneration
{
    public class SqlTableReference
    {
        public ISqlConnectionProvider SqlConnectionProvider { get; set; }
        public string TableName { get; set; }
        public string SchemaName { get; set; }

        public SqlTableReference(ISqlConnectionProvider sqlConnectionProvider, string qualifiedTableName)
        {
            SqlConnectionProvider = sqlConnectionProvider;
            var names = qualifiedTableName.Split(new[] {'.'}, StringSplitOptions.RemoveEmptyEntries);
            names.Length.Should().Be(2, "a schema and table name is expected -- use form similar to Schema.Table");
            SchemaName = names[0];
            TableName = names[1];
        }

        public SqlTableReference(ISqlConnectionProvider sqlConnectionProvider, string schemaName, string tableName)
        {
            SqlConnectionProvider = sqlConnectionProvider;
            SchemaName = schemaName;
            TableName = tableName;
        }
    }
}