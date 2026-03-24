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
using Cadeaubons_Presentation.Helpers;

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
            

            string email = TxtEmail.Text.Trim();
            string password = PwdPassword.Password;

            if (string.IsNullOrWhiteSpace(email))
            {
                MessageHelper.ShowWarning("Please enter your email.");
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                MessageHelper.ShowWarning("Please enter your password.");
                return;
            }

            try
            {
                // call naar Domaincontroller:
                var user = _domainController.login(email, password);

                if (user == null)
                {
                    MessageHelper.ShowError("Invalid email or password.");
                    return;
                }

                MessageHelper.ShowInfo($"Welcome {user.FirstName}!");


                // open customer screen if user.Role == Role.Customer
                // open admin screen if user.Role == Role.Admin
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError(ex.Message);
            }
        }

        //REGISTER-BUTTON
        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow registerWindow = new RegisterWindow(_domainController);
            registerWindow.Owner = this;
            registerWindow.ShowDialog();
        }


 



    }
}
