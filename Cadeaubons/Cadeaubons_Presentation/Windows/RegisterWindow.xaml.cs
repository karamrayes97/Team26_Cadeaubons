using Cadeaubons_Domain;
using Cadeaubons_Presentation.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace Cadeaubons_Presentation.Windows
{
    /// <summary>
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        private readonly DomainManager _dm;

        public RegisterWindow(DomainManager dm)
        {
            InitializeComponent();
            _dm = dm;
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            string firstName = TxtFirstName.Text.Trim();
            string lastName = TxtLastName.Text.Trim();
            string email = TxtEmail.Text.Trim();
            string phone = TxtPhoneNumber.Text.Trim();
            string password = PwdPassword.Password;
            string confirmPassword = PwdConfirmPassword.Password;

            if (string.IsNullOrWhiteSpace(firstName))
            {
                MessageHelper.ShowWarning("Voer je voornaam in.");
                return;
            }

            if (string.IsNullOrWhiteSpace(lastName))
            {
                MessageHelper.ShowWarning("Voer je achternaam in.");
                return;
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                MessageHelper.ShowWarning("Voer je e-mailadres in.");
                return;
            }

            if (DpDateOfBirth.SelectedDate == null)
            {
                MessageHelper.ShowWarning("Selecteer je geboortedatum.");
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                MessageHelper.ShowWarning("Voer een wachtwoord in.");
                return;
            }

            if (password.Length < 8)
            {
                MessageHelper.ShowWarning("Wachtwoord moet minstens 8 tekens lang zijn.");
                return;
            }

            if (password != confirmPassword)
            {
                MessageHelper.ShowWarning("Wachtwoorden komen niet overeen.");
                return;
            }

            try
            {
                var request = new Cadeaubons_Domain.DTO.RegisterUserRequest(
                    firstName,
                    lastName,
                    phone,
                    email.ToLower(),
                    DpDateOfBirth.SelectedDate.Value,
                    password
                );

                _dm.RegisterUser(request);

                MessageHelper.ShowInfo("Registratie geslaagd! Je kan nu aanmelden.");

                // Go back to login screen
                this.DialogResult = true;
                this.Close();
            }
            catch (InvalidOperationException ex)
            {
                MessageHelper.ShowError(ex.Message);
            }
            catch (ArgumentException ex)
            {
                MessageHelper.ShowError(ex.Message);
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError(ex.Message);
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            StartWindow window = new StartWindow(_dm);
            window.Show();
            this.Close();
        }

       
    }
}