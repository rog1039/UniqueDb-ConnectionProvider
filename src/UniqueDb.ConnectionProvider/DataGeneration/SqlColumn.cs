using System;
using System.Collections.Generic;
using System.Data;

namespace UniqueDb.ConnectionProvider.DataGeneration
{
    public class SqlColumn
    {
        public string Name { get; set; }
        public int OrdinalPosition { get; set; }
        public SqlType SqlDataType { get; set; }
        public bool IsNullable { get; set; }
        public string Default { get; set; }
    }
}