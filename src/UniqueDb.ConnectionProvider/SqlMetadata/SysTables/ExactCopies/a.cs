namespace UniqueDb.ConnectionProvider.SqlMetadata.SysTables.ExactCopies;

public class SysObjects_Exact
{
   public string   name                { get; set; }
   public int      object_id           { get; set; }
   public int?     principal_id        { get; set; }
   public int      schema_id           { get; set; }
   public int      parent_object_id    { get; set; }
   public string?  type                { get; set; }
   public string?  type_desc           { get; set; }
   public DateTime create_date         { get; set; }
   public DateTime modify_date         { get; set; }
   public bool     is_ms_shipped       { get; set; }
   public bool     is_published        { get; set; }
   public bool     is_schema_published { get; set; }
}

public class SysTables_Exact
{
   public string   name                               { get; set; }
   public int      object_id                          { get; set; }
   public int?     principal_id                       { get; set; }
   public int      schema_id                          { get; set; }
   public int      parent_object_id                   { get; set; }
   public string?  type                               { get; set; }
   public string?  type_desc                          { get; set; }
   public DateTime create_date                        { get; set; }
   public DateTime modify_date                        { get; set; }
   public bool     is_ms_shipped                      { get; set; }
   public bool     is_published                       { get; set; }
   public bool     is_schema_published                { get; set; }
   public int      lob_data_space_id                  { get; set; }
   public int?     filestream_data_space_id           { get; set; }
   public int      max_column_id_used                 { get; set; }
   public bool     lock_on_bulk_load                  { get; set; }
   public bool?    uses_ansi_nulls                    { get; set; }
   public bool?    is_replicated                      { get; set; }
   public bool?    has_replication_filter             { get; set; }
   public bool?    is_merge_published                 { get; set; }
   public bool?    is_sync_tran_subscribed            { get; set; }
   public bool     has_unchecked_assembly_data        { get; set; }
   public int?     text_in_row_limit                  { get; set; }
   public bool?    large_value_types_out_of_row       { get; set; }
   public bool?    is_tracked_by_cdc                  { get; set; }
   public byte?    lock_escalation                    { get; set; }
   public string?  lock_escalation_desc               { get; set; }
   public bool?    is_filetable                       { get; set; }
   public bool?    is_memory_optimized                { get; set; }
   public byte?    durability                         { get; set; }
   public string?  durability_desc                    { get; set; }
   public byte?    temporal_type                      { get; set; }
   public string?  temporal_type_desc                 { get; set; }
   public int?     history_table_id                   { get; set; }
   public bool?    is_remote_data_archive_enabled     { get; set; }
   public bool     is_external                        { get; set; }
   public int?     history_retention_period           { get; set; }
   public int?     history_retention_period_unit      { get; set; }
   public string?  history_retention_period_unit_desc { get; set; }
   public bool?    is_node                            { get; set; }
   public bool?    is_edge                            { get; set; }
   public int?     data_retention_period              { get; set; }
   public int?     data_retention_period_unit         { get; set; }
   public string?  data_retention_period_unit_desc    { get; set; }
   public byte?    ledger_type                        { get; set; }
   public string?  ledger_type_desc                   { get; set; }
   public int?     ledger_view_id                     { get; set; }
   public bool?    is_dropped_ledger_table            { get; set; }
}

public class SysColumn_Exact
{
   public int     object_id                           { get; set; }
   public string? name                                { get; set; }
   public int     column_id                           { get; set; }
   public byte    system_type_id                      { get; set; }
   public int     user_type_id                        { get; set; }
   public short   max_length                          { get; set; }
   public byte    precision                           { get; set; }
   public byte    scale                               { get; set; }
   public string? collation_name                      { get; set; }
   public bool?   is_nullable                         { get; set; }
   public bool    is_ansi_padded                      { get; set; }
   public bool    is_rowguidcol                       { get; set; }
   public bool    is_identity                         { get; set; }
   public bool?   is_computed                         { get; set; }
   public bool    is_filestream                       { get; set; }
   public bool?   is_replicated                       { get; set; }
   public bool?   is_non_sql_subscribed               { get; set; }
   public bool?   is_merge_published                  { get; set; }
   public bool?   is_dts_replicated                   { get; set; }
   public bool    is_xml_document                     { get; set; }
   public int     xml_collection_id                   { get; set; }
   public int     default_object_id                   { get; set; }
   public int     rule_object_id                      { get; set; }
   public bool?   is_sparse                           { get; set; }
   public bool?   is_column_set                       { get; set; }
   public byte?   generated_always_type               { get; set; }
   public string? generated_always_type_desc          { get; set; }
   public int?    encryption_type                     { get; set; }
   public string? encryption_type_desc                { get; set; }
   public string? encryption_algorithm_name           { get; set; }
   public int?    column_encryption_key_id            { get; set; }
   public string? column_encryption_key_database_name { get; set; }
   public bool?   is_hidden                           { get; set; }
   public bool    is_masked                           { get; set; }
   public int?    graph_type                          { get; set; }
   public string? graph_type_desc                     { get; set; }
   public bool?   is_data_deletion_filter_column      { get; set; }
   public int?    ledger_view_column_type             { get; set; }
   public string? ledger_view_column_type_desc        { get; set; }
   public bool?   is_dropped_ledger_column            { get; set; }
}

public class SysIndexes_Exact
{
   public int     object_id                   { get; set; }
   public string? name                        { get; set; }
   public int     index_id                    { get; set; }
   public byte    type                        { get; set; }
   public string? type_desc                   { get; set; }
   public bool?   is_unique                   { get; set; }
   public int?    data_space_id               { get; set; }
   public bool?   ignore_dup_key              { get; set; }
   public bool?   is_primary_key              { get; set; }
   public bool?   is_unique_constraint        { get; set; }
   public byte    fill_factor                 { get; set; }
   public bool?   is_padded                   { get; set; }
   public bool?   is_disabled                 { get; set; }
   public bool?   is_hypothetical             { get; set; }
   public bool?   is_ignored_in_optimization  { get; set; }
   public bool?   allow_row_locks             { get; set; }
   public bool?   allow_page_locks            { get; set; }
   public bool?   has_filter                  { get; set; }
   public string? filter_definition           { get; set; }
   public int?    compression_delay           { get; set; }
   public bool?   suppress_dup_key_messages   { get; set; }
   public bool?   auto_created                { get; set; }
   public bool?   optimize_for_sequential_key { get; set; }
}

public class SysIndex_Columns
{
   public int   object_id                  { get; set; }
   public int   index_id                   { get; set; }
   public int   index_column_id            { get; set; }
   public int   column_id                  { get; set; }
   public byte  key_ordinal                { get; set; }
   public byte  partition_ordinal          { get; set; }
   public bool? is_descending_key          { get; set; }
   public bool? is_included_column         { get; set; }
   public byte  column_store_order_ordinal { get; set; }
}

public class SysComputed_Column_Exact
{
   public int     object_id                           { get; set; }
   public string? name                                { get; set; }
   public int     column_id                           { get; set; }
   public byte    system_type_id                      { get; set; }
   public int     user_type_id                        { get; set; }
   public short   max_length                          { get; set; }
   public byte    precision                           { get; set; }
   public byte    scale                               { get; set; }
   public string? collation_name                      { get; set; }
   public bool?   is_nullable                         { get; set; }
   public bool    is_ansi_padded                      { get; set; }
   public bool    is_rowguidcol                       { get; set; }
   public bool    is_identity                         { get; set; }
   public bool    is_filestream                       { get; set; }
   public bool?   is_replicated                       { get; set; }
   public bool?   is_non_sql_subscribed               { get; set; }
   public bool?   is_merge_published                  { get; set; }
   public bool?   is_dts_replicated                   { get; set; }
   public bool    is_xml_document                     { get; set; }
   public int     xml_collection_id                   { get; set; }
   public int     default_object_id                   { get; set; }
   public int     rule_object_id                      { get; set; }
   public string? definition                          { get; set; }
   public bool    uses_database_collation             { get; set; }
   public bool    is_persisted                        { get; set; }
   public bool?   is_computed                         { get; set; }
   public bool    is_sparse                           { get; set; }
   public bool    is_column_set                       { get; set; }
   public byte?   generated_always_type               { get; set; }
   public string? generated_always_type_desc          { get; set; }
   public int?    encryption_type                     { get; set; }
   public string? encryption_type_desc                { get; set; }
   public string? encryption_algorithm_name           { get; set; }
   public int?    column_encryption_key_id            { get; set; }
   public string? column_encryption_key_database_name { get; set; }
   public bool    is_hidden                           { get; set; }
   public bool    is_masked                           { get; set; }
   public int?    graph_type                          { get; set; }
   public string? graph_type_desc                     { get; set; }
   public bool?   is_data_deletion_filter_column      { get; set; }
   public int?    ledger_view_column_type             { get; set; }
   public string? ledger_view_column_type_desc        { get; set; }
   public bool?   is_dropped_ledger_column            { get; set; }
}

public class SysCheck_Constraints_Exact
{
   public string   name                    { get; set; }
   public int      object_id               { get; set; }
   public int?     principal_id            { get; set; }
   public int      schema_id               { get; set; }
   public int      parent_object_id        { get; set; }
   public string?  type                    { get; set; }
   public string?  type_desc               { get; set; }
   public DateTime create_date             { get; set; }
   public DateTime modify_date             { get; set; }
   public bool     is_ms_shipped           { get; set; }
   public bool     is_published            { get; set; }
   public bool     is_schema_published     { get; set; }
   public bool     is_disabled             { get; set; }
   public bool     is_not_for_replication  { get; set; }
   public bool     is_not_trusted          { get; set; }
   public int      parent_column_id        { get; set; }
   public string?  definition              { get; set; }
   public bool?    uses_database_collation { get; set; }
   public bool     is_system_named         { get; set; }
}

public class SysDefault_Constaints_Exact
{
   public string   name                { get; set; }
   public int      object_id           { get; set; }
   public int?     principal_id        { get; set; }
   public int      schema_id           { get; set; }
   public int      parent_object_id    { get; set; }
   public string?  type                { get; set; }
   public string?  type_desc           { get; set; }
   public DateTime create_date         { get; set; }
   public DateTime modify_date         { get; set; }
   public bool     is_ms_shipped       { get; set; }
   public bool     is_published        { get; set; }
   public bool     is_schema_published { get; set; }
   public int      parent_column_id    { get; set; }
   public string?  definition          { get; set; }
   public bool     is_system_named     { get; set; }
}

public class SysForeign_Keys_Exact
{
   public string   name                           { get; set; }
   public int      object_id                      { get; set; }
   public int?     principal_id                   { get; set; }
   public int      schema_id                      { get; set; }
   public int      parent_object_id               { get; set; }
   public string?  type                           { get; set; }
   public string?  type_desc                      { get; set; }
   public DateTime create_date                    { get; set; }
   public DateTime modify_date                    { get; set; }
   public bool     is_ms_shipped                  { get; set; }
   public bool     is_published                   { get; set; }
   public bool     is_schema_published            { get; set; }
   public int?     referenced_object_id           { get; set; }
   public int?     key_index_id                   { get; set; }
   public bool     is_disabled                    { get; set; }
   public bool     is_not_for_replication         { get; set; }
   public bool     is_not_trusted                 { get; set; }
   public byte?    delete_referential_action      { get; set; }
   public string?  delete_referential_action_desc { get; set; }
   public byte?    update_referential_action      { get; set; }
   public string?  update_referential_action_desc { get; set; }
   public bool     is_system_named                { get; set; }
}

public class SysForeign_Key_Columns_Exact
{
   public int constraint_object_id { get; set; }
   public int constraint_column_id { get; set; }
   public int parent_object_id     { get; set; }
   public int parent_column_id     { get; set; }
   public int referenced_object_id { get; set; }
   public int referenced_column_id { get; set; }
}

public class SysIdentity_Column_Exact
{
   public int     object_id                           { get; set; }
   public string? name                                { get; set; }
   public int     column_id                           { get; set; }
   public byte    system_type_id                      { get; set; }
   public int     user_type_id                        { get; set; }
   public short   max_length                          { get; set; }
   public byte    precision                           { get; set; }
   public byte    scale                               { get; set; }
   public string? collation_name                      { get; set; }
   public bool?   is_nullable                         { get; set; }
   public bool    is_ansi_padded                      { get; set; }
   public bool    is_rowguidcol                       { get; set; }
   public bool    is_identity                         { get; set; }
   public bool    is_filestream                       { get; set; }
   public bool?   is_replicated                       { get; set; }
   public bool?   is_non_sql_subscribed               { get; set; }
   public bool?   is_merge_published                  { get; set; }
   public bool?   is_dts_replicated                   { get; set; }
   public bool    is_xml_document                     { get; set; }
   public int     xml_collection_id                   { get; set; }
   public int     default_object_id                   { get; set; }
   public int     rule_object_id                      { get; set; }
   public object? seed_value                          { get; set; }
   public object? increment_value                     { get; set; }
   public object? last_value                          { get; set; }
   public bool?   is_not_for_replication              { get; set; }
   public bool    is_computed                         { get; set; }
   public bool    is_sparse                           { get; set; }
   public bool    is_column_set                       { get; set; }
   public byte?   generated_always_type               { get; set; }
   public string? generated_always_type_desc          { get; set; }
   public int?    encryption_type                     { get; set; }
   public string? encryption_type_desc                { get; set; }
   public string? encryption_algorithm_name           { get; set; }
   public int?    column_encryption_key_id            { get; set; }
   public string? column_encryption_key_database_name { get; set; }
   public bool    is_hidden                           { get; set; }
   public bool    is_masked                           { get; set; }
   public int?    graph_type                          { get; set; }
   public string? graph_type_desc                     { get; set; }
   public bool?   is_data_deletion_filter_column      { get; set; }
   public int?    ledger_view_column_type             { get; set; }
   public string? ledger_view_column_type_desc        { get; set; }
   public bool?   is_dropped_ledger_column            { get; set; }
}

/// <summary>
/// Just adds to the Indexes table, if the key is system-named, and also records what number the index is in the table.
/// </summary>
public class SysKey_Constraints_Exact
{
   public string   name                { get; set; }
   public int      object_id           { get; set; }
   public int?     principal_id        { get; set; }
   public int      schema_id           { get; set; }
   public int      parent_object_id    { get; set; }
   public string?  type                { get; set; }
   public string?  type_desc           { get; set; }
   public DateTime create_date         { get; set; }
   public DateTime modify_date         { get; set; }
   public bool     is_ms_shipped       { get; set; }
   public bool     is_published        { get; set; }
   public bool     is_schema_published { get; set; }
   public int?     unique_index_id     { get; set; }
   public bool     is_system_named     { get; set; }
   public bool?    is_enforced         { get; set; }
}

public class SysSequences_Exact
{
   public string   name                { get; set; }
   public int      object_id           { get; set; }
   public int?     principal_id        { get; set; }
   public int      schema_id           { get; set; }
   public int      parent_object_id    { get; set; }
   public string?  type                { get; set; }
   public string?  type_desc           { get; set; }
   public DateTime create_date         { get; set; }
   public DateTime modify_date         { get; set; }
   public bool     is_ms_shipped       { get; set; }
   public bool     is_published        { get; set; }
   public bool     is_schema_published { get; set; }
   public object   start_value         { get; set; }
   public object   increment           { get; set; }
   public object   minimum_value       { get; set; }
   public object   maximum_value       { get; set; }
   public bool?    is_cycling          { get; set; }
   public bool?    is_cached           { get; set; }
   public int?     cache_size          { get; set; }
   public byte     system_type_id      { get; set; }
   public int      user_type_id        { get; set; }
   public byte     precision           { get; set; }
   public byte?    scale               { get; set; }
   public object   current_value       { get; set; }
   public bool     is_exhausted        { get; set; }
   public object?  last_used_value     { get; set; }
}

public class SysTriggers_Exact
{
   public string   name                   { get; set; }
   public int      object_id              { get; set; }
   public byte     parent_class           { get; set; }
   public string?  parent_class_desc      { get; set; }
   public int      parent_id              { get; set; }
   public string   type                   { get; set; }
   public string?  type_desc              { get; set; }
   public DateTime create_date            { get; set; }
   public DateTime modify_date            { get; set; }
   public bool     is_ms_shipped          { get; set; }
   public bool     is_disabled            { get; set; }
   public bool     is_not_for_replication { get; set; }
   public bool     is_instead_of_trigger  { get; set; }
}

public class SysViews_Exact
{
   public string   name                        { get; set; }
   public int      object_id                   { get; set; }
   public int?     principal_id                { get; set; }
   public int      schema_id                   { get; set; }
   public int      parent_object_id            { get; set; }
   public string?  type                        { get; set; }
   public string?  type_desc                   { get; set; }
   public DateTime create_date                 { get; set; }
   public DateTime modify_date                 { get; set; }
   public bool     is_ms_shipped               { get; set; }
   public bool     is_published                { get; set; }
   public bool     is_schema_published         { get; set; }
   public bool?    is_replicated               { get; set; }
   public bool?    has_replication_filter      { get; set; }
   public bool     has_opaque_metadata         { get; set; }
   public bool     has_unchecked_assembly_data { get; set; }
   public bool     with_check_option           { get; set; }
   public bool     is_date_correlation_view    { get; set; }
   public bool?    is_tracked_by_cdc           { get; set; }
   public bool?    has_snapshot                { get; set; }
   public byte?    ledger_view_type            { get; set; }
   public string?  ledger_view_type_desc       { get; set; }
   public bool?    is_dropped_ledger_view      { get; set; }
}