using Cadeaubons_Domain;
using Cadeaubons_Domain.DTO;
using System.Windows;

namespace Cadeaubons_Presentation.Windows
{
    public partial class PaymentResultWindow : Window
    {
        private readonly DomainManager _dm;
        private readonly UserDTO _currentUser;
        private readonly bool _success;

        public PaymentResultWindow(DomainManager dm, UserDTO currentUser, bool success)
        {
            InitializeComponent();
            _dm = dm;
            _currentUser = currentUser;
            _success = success;

            if (success)
            {
                ResultIcon.Text = "✅";
                ResultTitle.Text = "Betaling geslaagd!";
                ResultMessage.Text = "Je cadeaubon is succesvol aangemaakt en is nu beschikbaar in je overzicht.";
                BtnAction.Content = "Bekijk mijn cadeaubonnen";
            }
            else
            {
                ResultIcon.Text = "❌";
                ResultTitle.Text = "Betaling mislukt";
                ResultMessage.Text = "Er is iets misgegaan met je betaling. Probeer het opnieuw of contacteer de klantendienst.";
                BtnAction.Content = "Probeer opnieuw";
            }
        }

        private void BtnAction_Click(object sender, RoutedEventArgs e)
        {
            if (_success)
            {
                MyVouchersWindow window = new MyVouchersWindow(_dm, _currentUser);
                window.Show();
            }
            else
            {
                BuyGiftCardWindow window = new BuyGiftCardWindow(_dm, _currentUser);
                window.Show();
            }
            this.Close();
        }
    }
}