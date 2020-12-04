using System;
using System.Windows;
using UniqueDb.CSharpClassGenerator.Features.MainShell;

namespace UniqueDb.CSharpClassGenerator.Infrastructure
{
    public class MyApp
    {
        public void Start()
        {
            try
            {
                var shell = new MainShell()
                {
                    DataContext = new MainShellController()
                };
                var window2 = new Window()
                {
                    Width = 800,
                    Height = 1200,
                    Content = shell
                };
                window2.Show();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }
    }
}
