using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using UniqueDb.ConnectionProvider.DataGeneration;
using Xunit;

namespace UniqueDb.ConnectionProvider.Tests.DataGeneration
{
    public class AdoSchemaTableHelperTests
    {
        [Fact()]
        [Trait("Category", "Integration")]
        public void Get_Ado_SchemaTable_ColumnInformation()
        {
            var query = "SELECT 1";
            var dataColumns = AdoSchemaTableHelper.GetAdoSchemaTableColumns(LiveDbTestingSqlProvider.AdventureWorksDb, query);

            var cSharpProperties = dataColumns.Select(DataColumnToCSharpPropertyGenerator.ToCSharpProperty).ToList();
            var cSharpClass = GetCSharpClassFromAdoSchemaTableColumns(cSharpProperties);

            Console.WriteLine((string) cSharpClass);
            cSharpProperties.PrintStringTable();
        }

        private static string GetCSharpClassFromAdoSchemaTableColumns(IList<CSharpProperty> cSharpProperties)
        {
            var cSharpClass = CSharpClassGenerator.GenerateClassText("SysTypes", cSharpProperties);
            var compileResult = RoslynHelper.TryCompile(cSharpClass);
            compileResult.IsValid().Should().BeTrue();
            return cSharpClass;
        }
    }
}