using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniqueDb.ConnectionProvider.DataGeneration
{
    public static class CSharpClassGeneratorFromSqlTable
    {
        public static string GenerateClass(SqlTable table, string className = null)
        {
            var cSharpColumns = table.SqlColumns.Select(SqlColumnToCSharpPropertyGenerator.ToCSharpProperty).ToList();
            className = className ?? table.Name;
            var cSharpClass = CSharpClassGenerator.GenerateClassText(className, cSharpColumns);
            return cSharpClass;
        }
    }
}