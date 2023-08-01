So we have a few classes that are mostly straight up sys schema tables:
 - SysTable (objects)
 - SysColumn (root)
 - SysIndex (root)
 - SysIndexColumn (root)
 
# Sys Schema Tables
- sys.objects
- sys.tables (objects) *
- sys.columns (root) *
- sys.indexes (root) * 
- sys.index_columns (root) *
- sys.computer_columns (columns) *
- sys.check_constraints (objects) *
- sys.default_constraints (objects) * 
- sys.foreign_keys (objects) * 
- sys.foreign_key_columns (root) *
- sys.identity_columns (columns) *
- sys.key_constraints (objects) - includes PK & UQ (unique) *
- sys.sequences (objects) *
- sys.triggers (root) *
- sys.views (objects) *

Notes to self:
Tables are in sys.objects with their object_id
Columns are not in sys.tables - they are keyed by their object_id (table) and column_id
Indexes are not in sys.tables - they have an object_id that points to their table
Foreign keys are in sys.tables - have a parent_object_id that points to their table

# Hierarchical Organization for easy use in C#
- TableDef
  - ColumnDef
    - sys.columns
    - sys.computed_columns
    - sys.identity_columns
  - IdentityDef
    - sys.identity_columns
  - IndexDef
    - sys.indexes
    - IndexColumnDef
      - sys.index_columns
  - CheckConstraint
    - sys.check_constraints
  - DefaultConstraint
    - sys.default_constraints
    - Sequence
      - sys.sequences
  - ForeignKey
    - sys.foreign_keys
    - sys.foreign_key_columns
  - KeyConstraint
    - sys.key_constraint~~~~s
  - Triggers
    - sys.triggers
- ViewDef
- Sequences