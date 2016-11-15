﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Rogero.ReactiveProperty;
using UniqueDb.ConnectionProvider;
using UniqueDb.ConnectionProvider.DataGeneration.CSharpGeneration;
using UniqueDb.CSharpClassGenerator.Infrastructure;

namespace UniqueDb.CSharpClassGenerator.Features.CodeGen
{
    public class CodeGenController
    {
        public ReactiveProperty<SqlConnectionHolder> SelectedSqlConnection { get; } = new ReactiveProperty<SqlConnectionHolder>();
        public ObservableCollection<SqlConnectionHolder> SqlConnections { get; } = new ObservableCollection<SqlConnectionHolder>(SqlConnectionHolders.All);
        public ReactiveProperty<string> ClassName { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<string> SqlQuery { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<string> GeneratedCode { get; } = new ReactiveProperty<string>();

        public DelegateCommand<object> GenerateCodeCommand { get; private set; }
        public DelegateCommand<object> CopyCodeCommand { get; private set; }

        public CodeGenController()
        {
            CopyCodeCommand = new DelegateCommand<object>(CopyCode, CanCopyCode);
            GenerateCodeCommand = new DelegateCommand<object>(GenerateCode, CanGenerateCode);
            SelectedSqlConnection.Value = SqlConnections.Skip(1).First();
        }

        private void CopyCode(object obj)
        {
            Clipboard.SetText(GeneratedCode.Value);
        }

        private bool CanCopyCode(object arg)
        {
            return !string.IsNullOrWhiteSpace(GeneratedCode.Value);
        }

        private void GenerateCode(object o)
        {
            try
            {
                var sqlConnectionProvider = SelectedSqlConnection.Value.SqlConnectionProvider;
                var cSharpClass = CSharpClassGeneratorFromAdoDataReader.GenerateClass(sqlConnectionProvider, SqlQuery, ClassName.Value);
                GeneratedCode.Value = cSharpClass;
            }
            catch (Exception e)
            {
                MessageBox.Show(string.Join(Environment.NewLine, e.Message, e.StackTrace, e.ToString()));
            }
        }

        private bool CanGenerateCode(object o)
        {
            return SelectedSqlConnection.Value != null 
                && !string.IsNullOrWhiteSpace(ClassName.Value) 
                && !string.IsNullOrWhiteSpace(SqlQuery.Value);
        }
    }

    public class SqlConnectionHolder
    {
        public string Name { get; set; }
        public ISqlConnectionProvider SqlConnectionProvider { get; set; }
    }

    public class SqlConnectionHolders
    {

        public static SqlConnectionHolder Epicor905Test = new SqlConnectionHolder()
        {
            Name = "Epicor905 Test",
            SqlConnectionProvider = new StaticSqlConnectionProvider("epicor905", "EpicorTest905")
        };
        public static SqlConnectionHolder PbsiDatabase = new SqlConnectionHolder()
        {
            Name = "PBSI Database",
            SqlConnectionProvider = new StaticSqlConnectionProvider("ws2012sqlexp1\\sqlexpress", "PbsiDatabase")
        };
        public static SqlConnectionHolder PbsiCopy = new SqlConnectionHolder()
        {
            Name = "PBSI Copy",
            SqlConnectionProvider = new StaticSqlConnectionProvider("ws2012sqlexp1\\sqlexpress", "PbsiCopy")
        };
        public static IList<SqlConnectionHolder> All = new List<SqlConnectionHolder>()
        {
            Epicor905Test, PbsiDatabase, PbsiCopy
        };
    }
}