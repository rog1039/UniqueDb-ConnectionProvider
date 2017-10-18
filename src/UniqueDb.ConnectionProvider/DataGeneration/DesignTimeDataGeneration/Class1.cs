using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace UniqueDb.ConnectionProvider.DataGeneration.DesignTimeDataGeneration
{
    public static class DesignTimeDataCodeTemplate
    {
        public const string TemplateText = @"
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DesignTimeData
{
    public static class %ClassName%DesignTimeData
    {
        private const string %ClassName%DataCustomJson = @""%Json%"";
        
        public static %ClassName% Get1()
        {
            return Get().Take(1).First();
        }

        public static IList<%ClassName%> Get()
        {
            var normalJson = %ClassName%DataCustomJson.Replace(""`~`"", ""\"""");
            var result = JsonConvert.DeserializeObject<List<%ClassName%>>(normalJson);
            return result;
        }
    }

    %ClassDeclaration%
}
            ";

        public static string CreateCode(string className, string json, string classDeclaration)
        {
            var text = TemplateText.Replace($"%ClassName%", className);
            var escapedJson = json.EscapeJson();
            text = text.Replace("%Json%", escapedJson);
            text = text.Replace($"%ClassDeclaration%", classDeclaration);
            return text;
        }
    }

    public static class CustomJsonConvert
    {
        private const string JsonQuoteCharacterReplacement = "`~`";

        public static string ToJson(object o, Formatting formatting = Formatting.None)
        {
            var json = JsonConvert.SerializeObject(o, formatting);
            json = json.Replace("\"", JsonQuoteCharacterReplacement);
            return json;
        }

        public static T ToObject<T>(string json)
        {
            json = json.Replace(JsonQuoteCharacterReplacement, "\"");
            var o = JsonConvert.DeserializeObject<List<T>>(json);
            return o.Single();
        }

        public static IList<T> ToObjectList<T>(string json)
        {
            json = json.Replace(JsonQuoteCharacterReplacement, "\"");
            var o = JsonConvert.DeserializeObject<List<T>>(json);
            return o;
        }

        public static string EscapeJson(this string json) => json.Replace("\"", JsonQuoteCharacterReplacement);

        public static T To<T>(this string s) => ToObject<T>(s);
        public static IList<T> ToListOf<T>(this string s) => ToObjectList<T>(s);
        public static void ToConsole(this string s) => Console.WriteLine(s);
        public static string ToCustomJson(this object o, Formatting formatting = Formatting.None) => ToJson(o, formatting);
    }
}
