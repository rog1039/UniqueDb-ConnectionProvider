using System.Data.Common;
using System.Windows;

namespace UniqueDb.CSharpClassGenerator.Features.DatabaseSelection;

public static class DbConnectionTester
{
    public static void TestConnection(DbConnection conn)
    {
        try
        {
            conn.Open();
            MessageBox.Show("Connected successfully.");
        }
        catch (Exception e)
        {
            MessageBox.Show("Connected failed." + e);
        }
    }
}