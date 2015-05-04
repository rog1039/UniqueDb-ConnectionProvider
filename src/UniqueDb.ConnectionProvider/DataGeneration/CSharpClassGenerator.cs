using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniqueDb.ConnectionProvider.DataGeneration
{
    public static class CSharpClassGenerator
    {
        public static string GenerateClass(SqlTable table)
        {
            var cSharpColumns = table.SqlColumns.Select(SqlColumnToCSharpPropertyGenerator.ToCSharpProperty).ToList();
            var cSharpClass = GenerateClassText(table.Name, cSharpColumns);
            return cSharpClass;
        }

        public static string GenerateClassText(string className, IEnumerable<CSharpProperty> cSharpProperties)
        {
            var sb = new StringBuilder();
            sb.AppendLine(string.Format("public class {0}", className));
            sb.AppendLine("{");
            foreach (var cSharpProperty in cSharpProperties)
            {
                sb.AppendLine("    " + cSharpProperty.ToString());
            }
            sb.AppendLine("}");
            return sb.ToString();
        }
    }
}