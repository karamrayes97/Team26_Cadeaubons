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
                MessageHelper.ShowWarning("Please enter your first name.");
                return;
            }

            if (string.IsNullOrWhiteSpace(lastName))
            {
                MessageHelper.ShowWarning("Please enter your last name.");
                return;
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                MessageHelper.ShowWarning("Please enter your email.");
                return;
            }

            if (DpDateOfBirth.SelectedDate == null)
            {
                MessageHelper.ShowWarning("Please select your date of birth.");
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                MessageHelper.ShowWarning("Please enter a password.");
                return;
            }

            if (password.Length < 8)
            {
                MessageHelper.ShowWarning("Password must be at least 8 characters long.");
                return;
            }

            if (password != confirmPassword)
            {
                MessageHelper.ShowWarning("Passwords do not match.");
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

                MessageHelper.ShowInfo("Registration successful! You can now log in.");

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