namespace UniqueDb.ConnectionProvider.DataGeneration
{
    public abstract class SqlTypeConversion<TCSharp, TSql>
    {
        public string SqlType { get; set; }
        public string CSharpType { get; set; }

        public abstract TCSharp ToCSharp(TSql sqlValue);
        public abstract TSql ToSql(TCSharp cSharpValue);
    }
}