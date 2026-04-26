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
using Cadeaubons_Domain;
using Cadeaubons_Domain.DTO;

namespace Cadeaubons_Presentation.Windows
{
    /// <summary>
    /// Interaction logic for CustomerWindow.xaml
    /// </summary>
    public partial class CustomerWindow : Window
    {

        private readonly DomainManager _dm;
        private UserDTO _currentUser;
        public CustomerWindow(DomainManager dm, UserDTO currentUser)
        {
            InitializeComponent();
            _dm = dm;
            _currentUser = currentUser;
            TxtWelcome.Text = $"Welcome {_currentUser.FirstName} !";
        }

        private void BtnViewThemes_Click(object sender, RoutedEventArgs e)
        {
            ThemeListWindow window = new ThemeListWindow(_dm, _currentUser);
            window.Show();
            this.Close();
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            _currentUser = null;
            StartWindow window = new StartWindow(_dm); 
            window.Show();
            this.Close();
        }

		private void PurchaseButton_Click(object sender, RoutedEventArgs e)
		{
            BuyGiftCardWindow window = new BuyGiftCardWindow(_dm);
            window.Show();
            this.Close();
        }
    }
}
