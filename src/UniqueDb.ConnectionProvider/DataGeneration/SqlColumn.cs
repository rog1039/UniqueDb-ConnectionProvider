using System;
using System.Data;

namespace UniqueDb.ConnectionProvider.DataGeneration
{
    public class SqlColumn
    {
        public string Name { get; set; }
        public int OrdinalPosition { get; set; }
        public string SqlDataType { get; set; }
        public bool IsNullable { get; set; }
        public string Default { get; set; }
        public int? CharacterMaxLength { get; set; }
    }
}