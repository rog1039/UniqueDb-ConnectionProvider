using System;
using UniqueDb.ConnectionProvider.DataGeneration;
using Xunit;

namespace UniqueDb.ConnectionProvider.Tests.DataGeneration
{
    public class CreateCSharpClassForDescribeResultSetStoredProcedureQuery
    {
        [Fact()]
        [Trait("Category", "Integration")]
        public void CreateCSharpClassText()
        {
            var query = "sp_describe_first_result_set @tsql = N'SELECT object_id, name, type_desc FROM sys.indexes'";
            var cSharpClass =
                CSharpClassGeneratorFromAdoDataReader.GenerateClass(LiveDbTestingSqlProvider.AdventureWorksDb,
                    query, "DescribeResultSet");
            Console.WriteLine(cSharpClass);
        }
    }
}