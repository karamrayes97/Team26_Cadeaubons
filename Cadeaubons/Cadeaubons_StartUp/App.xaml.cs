using Cadeaubons_Presentation;
using Cadeaubons_Domain;
using System.Configuration;
using System.Data;
using System.DirectoryServices.ActiveDirectory;
using System.Windows;
using Cadeaubons_Domain.Repo;
using Cadeaubons_Domain.Services;
using System.Globalization;
using System.Threading;
using System.Windows.Markup;

namespace Cadeaubons.StartUp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // Forceer Nederlandstalige UI (€, datums dd/MM/yyyy, komma als decimaal)
            var culture = new CultureInfo("nl-BE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;

            // Zorg dat WPF bindings óók nl-BE gebruiken (anders blijft StringFormat=C de OS-culture pakken)
            FrameworkElement.LanguageProperty.OverrideMetadata(
                typeof(FrameworkElement),
                new FrameworkPropertyMetadata(
                    XmlLanguage.GetLanguage(culture.IetfLanguageTag)));

            Repository repository = new Repository();

            TestDataSeeder.Seed(repository);


            UserService userService = new UserService(repository);
            CityService cityService = new CityService(repository);
            StoreService storeService = new StoreService(repository);
            ThemeService themeService = new ThemeService(repository);
            VoucherService voucherService = new VoucherService(repository);
            PaymentService paymentService = new PaymentService(repository);
            ConsumptionService consumptionService = new ConsumptionService(repository);
            DomainManager domainManager = new DomainManager(userService,cityService,storeService, themeService,voucherService, paymentService, consumptionService);

            CadeaubonsApplication application = new(domainManager);
            application.Start();
        }
    }

}
