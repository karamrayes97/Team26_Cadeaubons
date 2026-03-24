
using System.DirectoryServices.ActiveDirectory;
using Cadeaubons_Domain;
using Cadeaubons_Presentation.Windows;
using System.Windows;

namespace Cadeaubons_Presentation
{
    public class CadeaubonsApplication
    {
        private readonly DomainManager _domainManager;
        public CadeaubonsApplication(DomainManager domainManager)
        {
            _domainManager = domainManager;
        }
        public void Start()
        {
            StartWindow startWindow = new StartWindow(_domainManager);
            startWindow.Show();
        }
    }

}
