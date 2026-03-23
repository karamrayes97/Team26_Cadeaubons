using Cadeaubons_Presentation;
using Cadeaubons_Domain;
using System.Configuration;
using System.Data;
using System.DirectoryServices.ActiveDirectory;
using System.Windows;
using Cadeaubons_Domain.Repo;
using Cadeaubons_Domain.Services;

namespace Cadeaubons.StartUp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Repository repository = new Repository();
            UserService userService = new UserService(repository);
            DomainManager domainManager = new DomainManager(userService);

            CadeaubonsApplication application = new(domainManager);
            application.Start();
        }
    }

}
