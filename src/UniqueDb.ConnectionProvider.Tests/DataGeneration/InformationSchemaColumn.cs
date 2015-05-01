using System.Collections.Generic;
using System.Security.AccessControl;

namespace UniqueDb.ConnectionProvider.Tests.DataGeneration
{
    public class InformationSchemaColumn
    {
        public string TABLE_CATALOG { get; set; }
        public string TABLE_SCHEMA { get; set; }
        public string TABLE_NAME { get; set; }
        public string COLUMN_NAME { get; set; }
        public int ORDINAL_POSITION { get; set; }
        public string COLUMN_DEFAULT { get; set; }
        public string IS_NULLABLE { get; set; }
        public string DATA_TYPE { get; set; }
        public int? CHARACTER_MAXIMUM_LENGTH { get; set; }
        public int? CHARACTER_OCTET_LENGTH { get; set; }
        public int? NUMERIC_PRECISION { get; set; }
        public int? NUMERIC_PRECISION_RADIX { get; set; }
        public int? NUMERIC_SCALE { get; set; }
        public int? DATETIME_PRECISION { get; set; }
        public string CHARACTER_SET_CATALOG { get; set; }
        public string CHARACTER_SET_SCHEMA { get; set; }
        public string CHARACTER_SET_NAME { get; set; }
        public string COLLATION_CATALOG { get; set; }
        public string COLLATION_SCHEMA { get; set; }
        public string COLLATION_NAME { get; set; }
        public string DOMAIN_CATALOG { get; set; }
        public string DOMAIN_SCHEMA { get; set; }
        public string DOMAIN_NAME { get; set; }
    }

    public class InformationSchemaTable
    {
        public string TABLE_CATALOG { get; set; }
        public string TABLE_SCHEMA { get; set; }
        public string TABLE_NAME { get; set; }
        public string TABLE_TYPE { get; set; }
    }

    public class InformationSchemaTableDefinition
    {
        public InformationSchemaTable InformationSchemaTable { get; set; }
        public IList<InformationSchemaColumn> InformationSchemaColumns { get; set; }
    }
}