using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniqueDb.ConnectionProvider.DataGeneration
{
    public static class CSharpClassGeneratorFromSqlTable
    {
        public static string GenerateClass(SqlTable table, string className = null)
        {
            var cSharpProperties = table.SqlColumns
                .Select(CSharpPropertyFactoryFromSqlColumn.ToCSharpProperty)
                .ToList();
            className = className ?? table.Name;
            var cSharpClass = CSharpClassTextGenerator.GenerateClassText(className, cSharpProperties);
            return cSharpClass;
        }
    }
}