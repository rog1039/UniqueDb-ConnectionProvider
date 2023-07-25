using Newtonsoft.Json;

namespace UniqueDb.ConnectionProvider.SqlMetadata.InformationSchema;

public static class InformationSchemaTableDefinitionFromJson
{
    public static SISTableDefinition SampleTable()
    {
        var text            = File.ReadAllText("SampleTable_InformationSchemaTableDefinition_Json.txt");
        var tableDefinition = JsonConvert.DeserializeObject<SISTableDefinition>(text);
        return tableDefinition;
    }            
}