using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Autofac;
using UniqueDb.CSharpClassGenerator.Features.CodeGen;

namespace UniqueDb.CSharpClassGenerator.Infrastructure
{
    public class MyApp
    {
        public void Start()
        {
            var controller = new CodeGenController();
            var window = new Window() {Content = new CodeGenView()};
            window.DataContext = controller;
            window.Width = 800;
            window.Height = 1200;
            window.Show();
        }
    }

    public class ContainerProvider
    {
        public IContainer Get()
        {
            throw new NotImplementedException();
        }
    }
}
