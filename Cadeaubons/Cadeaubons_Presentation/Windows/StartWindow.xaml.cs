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
using Cadeaubons_Domain.DTO;

namespace Cadeaubons_Presentation.Windows
{
    /// <summary>
    /// Interaction logic for StartWindow.xaml
    /// </summary>
    public partial class StartWindow : Window
    {
        private readonly DomainManager _dm;

        public StartWindow(DomainManager dm)
        {
            InitializeComponent();
            _dm = dm;
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
                var user = _dm.Login(email, password);

                if (user == null)
                {
                    MessageHelper.ShowError("Invalid email or password.");
                    return;
                }

                if (user.Role == Cadeaubons_Domain.Model.Role.Customer )
                {   
                    CustomerWindow customerWindow = new CustomerWindow(_dm, user);                    
                    customerWindow.Show();
                    this.Close();
                }


                if (user.Role == Cadeaubons_Domain.Model.Role.Admin)
                {
                    AdminWindow adminWindow = new AdminWindow(_dm);
                    adminWindow.Show();
                    this.Close();
                }

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

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow window = new RegisterWindow(_dm);
            window.Show();
            this.Close();
        }
    }
}
