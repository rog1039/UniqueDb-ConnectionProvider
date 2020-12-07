using System;
using UniqueDb.ConnectionProvider.DataGeneration.CSharpGeneration;
using Xunit;

namespace UniqueDb.ConnectionProvider.Tests.DataGeneration
{
    public class CreateCSharpClassForDescribeResultSetStoredProcedureQuery
    {
        [Fact()]
        [Trait("Category", "Integration")]
        public void CreateClassForDescribeResultSet()
        {
            var query = "sp_describe_first_result_set @tsql = N'SELECT object_id, name, type_desc FROM sys.indexes'";
            var cSharpClass =
                CSharpClassGeneratorFromAdoDataReader.GenerateClass(SqlConnectionProviders.AdventureWorksDb,
                    query, "DescribeResultSetRow", CSharpClassTextGeneratorOptions.Default);
            Console.WriteLine(cSharpClass);
        }
        
        [Fact()]
        [Trait("Category", "Integration")]
        public void CreateClassForUserDefinedTypes()
        {
            var query = "select * from sys.types where is_user_defined = 1";
            var cSharpClass =
                CSharpClassGeneratorFromAdoDataReader.GenerateClass(SqlConnectionProviders.AdventureWorksDb,
                    query, "SqlSysType", CSharpClassTextGeneratorOptions.Default);
            Console.WriteLine(cSharpClass);
        }
    }
}