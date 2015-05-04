using System.Collections.Generic;

namespace UniqueDb.ConnectionProvider.DataGeneration
{
    public class InformationSchemaTableDefinition
    {
        public InformationSchemaTable InformationSchemaTable { get; set; }
        public IList<InformationSchemaColumn> InformationSchemaColumns { get; set; }
    }
}