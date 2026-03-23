using Cadeaubons_Domain;
using System;
using System.Collections.Generic;
using System.Text;
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
    /// Interaction logic for StartWindow.xaml
    /// </summary>
    public partial class StartWindow : Window
    {
        private readonly Domaincontroller _domainController;

        public StartWindow(Domaincontroller domainController)
        {
            InitializeComponent();
            _domainController = domainController;
        }

        // LOGIN-BUTTON:

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            TxtMessage.Text = string.Empty;

            string email = TxtEmail.Text.Trim();
            string password = PwdPassword.Password;

            if (string.IsNullOrWhiteSpace(email))
            {
                ShowWarning("Please enter your email.");
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                ShowWarning("Please enter your password.");
                return;
            }

            try
            {
                // call naar Domancontroller:
                var user = _domainController.login(email, password);

                if (user == null)
                {
                    ShowError("Invalid email or password.");
                    return;
                }

                ShowInfo($"Welcome {user.FirstName}!");


                // open customer screen if user.Role == Role.Customer
                // open admin screen if user.Role == Role.Admin
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Register screen will be implemented later", "Register", MessageBoxButton.OK, MessageBoxImage.Information);

            // Create register window later
        }


        //Error message-methods, can be made into one method later, and put in a different, central class

        private void ShowError(string message)
        {
            MessageBox.Show(
                message,
                "Error",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }

        private void ShowWarning(string message)
        {
            MessageBox.Show(
                message,
                "Warning",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
        }

        private void ShowInfo(string message)
        {
            MessageBox.Show(
                message,
                "Information",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }



    }
}
