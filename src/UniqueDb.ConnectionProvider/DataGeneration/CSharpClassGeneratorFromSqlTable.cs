using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniqueDb.ConnectionProvider.DataGeneration
{
    public static class CSharpClassGeneratorFromSqlTable
    {
        public static string GenerateClass(SqlTable table, string className = null)
        {
            var cSharpColumns = table.SqlColumns.Select(CSharpPropertyFactoryFromSqlColumn.ToCSharpProperty).ToList();
            className = className ?? table.Name;
            var cSharpClass = CSharpClassTextGenerator.GenerateClassText(className, cSharpColumns);
            return cSharpClass;
        }
    }
}