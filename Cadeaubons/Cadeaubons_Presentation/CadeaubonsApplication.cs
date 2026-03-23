
using System.DirectoryServices.ActiveDirectory;
using Cadeaubons_Domain;
using Cadeaubons_Presentation.Windows;
using System.Windows;

namespace Cadeaubons_Presentation
{
    public class CadeaubonsApplication
    {
        private readonly Domaincontroller _domainController;
        public CadeaubonsApplication(Domaincontroller domainController)
        {
            _domainController = domainController;
        }
        public void Start()
        {
            StartWindow startWindow = new StartWindow(_domainController);
            startWindow.Show();
        }
    }

}
