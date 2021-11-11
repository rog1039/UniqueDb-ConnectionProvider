using System.IO;
using Newtonsoft.Json;

namespace UniqueDb.ConnectionProvider.DataGeneration;

public static class InformationSchemaTableDefinitionFromJson
{
    public static InformationSchemaTableDefinition SampleTable()
    {
        var text            = File.ReadAllText("SampleTable_InformationSchemaTableDefinition_Json.txt");
        var tableDefinition = JsonConvert.DeserializeObject<InformationSchemaTableDefinition>(text);
        return tableDefinition;
    }            
}