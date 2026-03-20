using Cadeaubons_Presentation;
using System.Configuration;
using System.Data;
using System.DirectoryServices.ActiveDirectory;
using System.Windows;
using Cadeaubons_Domain;

namespace Cadeaubons.StartUp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Domaincontroller dm = new();

            CadeaubonsApplication application = new(dm);
        }
    }

}
