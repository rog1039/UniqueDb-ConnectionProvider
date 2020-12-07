using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using UniqueDb.ConnectionProvider.DataGeneration;
using UniqueDb.ConnectionProvider.DataGeneration.CSharpGeneration;
using UniqueDb.ConnectionProvider.DataGeneration.SqlMetadata;
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
            var dataColumns = AdoSchemaTableHelper.GetAdoSchemaDataColumns(SqlConnectionProviders.AdventureWorksDb, query);
            
            var cSharpProperties = dataColumns.Select(CSharpPropertyFactoryFromAdoSchemaTableDataColumn.ToCSharpProperty).ToList();
            var cSharpClass = GetCSharpClassFromAdoSchemaTableColumns(cSharpProperties);

            Console.WriteLine(cSharpClass);
            cSharpProperties.PrintStringTable();
        }

        private static string GetCSharpClassFromAdoSchemaTableColumns(IList<CSharpProperty> cSharpProperties)
        {
            var cSharpClass = CSharpClassTextGenerator.GenerateClassText("SysTypes", cSharpProperties, CSharpClassTextGeneratorOptions.Default);
            var compileResult = RoslynHelper.TryCompile(cSharpClass);
            compileResult.IsValid().Should().BeTrue();
            return cSharpClass;
        }
    }
}