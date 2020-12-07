using System.Collections.Generic;
using System.Linq;
using UniqueDb.ConnectionProvider.DataGeneration.CSharpGeneration;

namespace UniqueDb.ConnectionProvider.DataGeneration
{
    public class CSharpPropertyTextGenerator
    {
        private readonly CSharpProperty Property;
        private CSharpClassTextGeneratorOptions _cSharpClassTextGeneratorOptions = new CSharpClassTextGeneratorOptions();

        private static readonly List<string> TypesThatCanBeNullable = new List<string>()
        {
            "bool", "byte", "char", "decimal", "double", "enum", "float", "int", "long",
            "sbyte", "short", "struct",

            "uint", "ulong", "ushort",

            "UInt", "UInt16", "UInt32", "UInt64",
            "Int", "Int16", "Int32", "Int64",

            "datetime"
        }.Select(z => z.ToLower()).ToList();

        private static readonly Dictionary<string,string> ClrToCsharpTypeMap = new Dictionary<string, string>()
        {
            {"Boolean", "bool"   },
            {"Byte"   , "byte"   },
            {"SByte"  , "sbyte"  },
            {"Char"   , "char"   },
            {"Decimal", "decimal"},
            {"Double" , "double" },
            {"Single" , "float"  },
            {"Int32"  , "int"    },
            {"UInt32" , "uint"   },
            {"Int64"  , "long"   },
            {"UInt64" , "ulong"  },
            {"Object" , "object" },
            {"Int16"  , "short"  },
            {"UInt16" , "ushort" },
            {"String" , "string" }
        };
        
        public CSharpPropertyTextGenerator(CSharpProperty property)
        {
            Property = property;
        }

        public CSharpPropertyTextGenerator(CSharpProperty property, CSharpClassTextGeneratorOptions options) : this(property)
        {
            _cSharpClassTextGeneratorOptions = options;
        }

        public string Generate()
        {
            if (_cSharpClassTextGeneratorOptions.IncludePropertyAnnotationAttributes)
            {
                return
                    $"{AttributeText}    {AccessModifier} {DataTypeString} {PropertyName} {{{GetterAccessModifier} get;{SetterAccessModifier} set; }}";
            }
            return
                $"    {AccessModifier} {DataTypeString} {PropertyName} {{{GetterAccessModifier} get;{SetterAccessModifier} set; }}";
        }


        private string AttributeText => Property.DataAnnotationDefinitionBases.Aggregate(string.Empty, (s, @base) => s+"    "+@base.ToAttributeString()+"\r\n" );

        private string AccessModifier => Property.ClrAccessModifier.ToString().ToLower();

        public string DataTypeString => Property.IsNullable && TypesThatCanBeNullable.Contains(Property.DataType.ToLower())
            ? ConvertToCSharpTypeIfRequired(Property.DataType) + "?"
            : ConvertToCSharpTypeIfRequired(Property.DataType);

        public string ConvertToCSharpTypeIfRequired(string dataType)
        {
            string cSharpType;
            return ClrToCsharpTypeMap.TryGetValue(dataType, out cSharpType) 
                ? cSharpType 
                : dataType;
        }

        public string PropertyName => Property.Name;

        public string GetterAccessModifier
            =>
                Property.GetterClrAccessModifier != ClrAccessModifier.Public
                    ? " " + Property.GetterClrAccessModifier.ToString().ToLower()
                    : string.Empty;

        public string SetterAccessModifier
            =>
                Property.SetterClrAccessModifier != ClrAccessModifier.Public
                    ? " " + Property.SetterClrAccessModifier.ToString().ToLower()
                    : string.Empty;

    }
}