using System;
using System.ComponentModel.DataAnnotations;

namespace UniqueDb.ConnectionProvider.DataGeneration.SqlMetadata;

public class SqlSysType
{
    [StringLength(128)]
    public string name { get;          set; }
    public Byte  system_type_id { get; set; }
    public int   user_type_id   { get; set; }
    public int   schema_id      { get; set; }
    public int?  principal_id   { get; set; }
    public Int16 max_length     { get; set; }
    public Byte  precision      { get; set; }
    public Byte  scale          { get; set; }
    [StringLength(128)]
    public string collation_name { get;   set; }
    public bool? is_nullable       { get; set; }
    public bool  is_user_defined   { get; set; }
    public bool  is_assembly_type  { get; set; }
    public int   default_object_id { get; set; }
    public int   rule_object_id    { get; set; }
    public bool  is_table_type     { get; set; }
}