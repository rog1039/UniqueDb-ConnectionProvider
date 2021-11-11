using System;
using System.Collections.Generic;

namespace UniqueDb.ConnectionProvider.DataGeneration.SqlManipulation;
/*
 * This file holds the beginnings of a more robust implementation of Sql Create Table script creation.
 * The original file was not very customizable and everything was generally hardcoded with the assumption
 * that only a single Type was being converted into a table.
 *
 * As I looked into processing Type hierarchies into SQL Create table scripts, I realized that design was
 * more of a problem than a help. Below is the beginning of that journey.
 *
 * Since this is going to be a large task that could take a decent amount of time, I ended up using the
 * hack job I developed so far on the simple system and ended up  modifying parts of it by hand. The code
 * below is the start to a system that could be fully automated without any tweaking at the end.
 */

public class TableGenerationInputDataSimple
{
    public string SchemaName { get; set; }
    public string TableName  { get; set; }
    public Type   Type       { get; set; }
}

/// <summary>
/// Information for Table-Per-Hierarchy Generation
/// </summary>
public class TableGenerationInputDataHierarchy
{
    public string                  SchemaName              { get; set; }
    public string                  TableName               { get; set; }
    public Type                    RootType                { get; set; }
    public IList<Type>             Subtypes                { get; set; }
    public HierarchyModelingMethod HierarchyModelingMethod { get; set; }
}

public enum HierarchyModelingMethod
{
    TablePerHierarchy,
    TablePerType
}
    
public static class AdvancedCreateTableScriptCreation
{
    public static string BuildCreateScript(TableGenerationInputDataSimple inputData)
    {
        throw new NotImplementedException();   
    }
        
    public static string BuildCreateScript(TableGenerationInputDataHierarchy inputData)
    {
        throw new NotImplementedException();   
    }
}