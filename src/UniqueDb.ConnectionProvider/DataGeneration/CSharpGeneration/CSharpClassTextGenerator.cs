using System.Collections.Generic;
using System.Text;

namespace UniqueDb.ConnectionProvider.DataGeneration.CSharpGeneration
{
    public static class CSharpClassTextGenerator
    {
        public static string GenerateClassText(string className, IEnumerable<CSharpProperty> cSharpProperties)
        {
            var sb = new StringBuilder();
            sb.AppendLine(string.Format("public class {0}", className));
            sb.AppendLine("{");
            foreach (var cSharpProperty in cSharpProperties)
            {
                sb.AppendLine(cSharpProperty.ToString());
            }
            sb.AppendLine("}");
            return sb.ToString();
        }
    }
}