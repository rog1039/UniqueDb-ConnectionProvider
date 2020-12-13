using System.Windows;
using UniqueDb.CSharpClassGenerator.Infrastructure;

namespace UniqueDb.CSharpClassGenerator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var myApp = new MyApp();
            myApp.Start();
        }
    }
}