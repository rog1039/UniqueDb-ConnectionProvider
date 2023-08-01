namespace UniqueDb.ConnectionProvider.CSharpGeneration.DesignTimeDataGeneration;

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
      var text        = TemplateText.Replace($"%ClassName%", className);
      var escapedJson = json.EscapeJson();
      text = text.Replace("%Json%",              escapedJson);
      text = text.Replace($"%ClassDeclaration%", classDeclaration);
      return text;
   }
}