using System;
using System.Collections.Generic;

namespace UniqueDb.ConnectionProvider.DataGeneration
{
    public class SqlTable
    {
        public string Schema { get; set; }
        public string Name { get; set; }
        public IList<SqlColumn> SqlColumns { get; set; }
    }


    public class SyntaxParseResult
    {
        public SyntaxParseResult(string sqlTypeName, int? precision1, int? precision2, string input)
        {
            SqlTypeName = sqlTypeName;
            Precision1 = precision1;
            Precision2 = precision2;
            Input = input;
        }

        public SyntaxParseResult(string sqlTypeName, int? precision1, string input)
        {
            SqlTypeName = sqlTypeName;
            Precision1 = precision1;
            Input = input;
        }

        public SyntaxParseResult(string sqlTypeName, string text, string input)
        {
            SqlTypeName = sqlTypeName;
            Text = text;
            Input = input;
        }

        public SyntaxParseResult(string sqlTypeName, string input)
        {
            SqlTypeName = sqlTypeName;
            Input = input;
        }

        public string SqlTypeName { get; set; }
        public int? Precision1 { get; set; }
        public int? Precision2 { get; set; }
        public string Text { get; set; }
        public string Input { get; set; }
    }
}