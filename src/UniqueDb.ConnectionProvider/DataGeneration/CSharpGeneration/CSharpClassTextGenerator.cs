using System;
using System.Collections.Generic;
using System.Text;

namespace UniqueDb.ConnectionProvider.DataGeneration.CSharpGeneration
{
    public static class CSharpClassTextGenerator
    {
        public static string GenerateClassText(string className, IEnumerable<CSharpProperty> cSharpProperties, CSharpClassTextGeneratorOptions options)
        {
            var sb = new StringBuilder();
            sb.AppendLine(string.Format("public class {0}", className));
            sb.AppendLine("{");
            foreach (var cSharpProperty in cSharpProperties)
            {
                var transformedCSharpProperty = TransformProperty(className, cSharpProperty);
                sb.AppendLine(transformedCSharpProperty.ToString(options));
            }
            sb.AppendLine("}");
            return sb.ToString();
        }

        private static CSharpProperty TransformProperty(string className, CSharpProperty cSharpProperty)
        {
            var property = cSharpProperty.Copy();
            property.Name = property.Name.Replace(".", "_");
            if (property.Name.Equals(className))
            {
                property.Name = property.Name + "Property";
            }
            return property;
        }
    }

    public class CSharpClassTextGeneratorOptions
    {
        public bool IncludePropertyAnnotationAttributes { get; set; } = true;

        public static CSharpClassTextGeneratorOptions Default = new CSharpClassTextGeneratorOptions();
    }
}