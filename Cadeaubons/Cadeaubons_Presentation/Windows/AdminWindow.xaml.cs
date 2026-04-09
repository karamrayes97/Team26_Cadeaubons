using Cadeaubons_Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
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
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        private readonly DomainManager _dm;
        public AdminWindow(DomainManager dm)
        {
            InitializeComponent();
            _dm = dm;
        }

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			AddShopsWindow adminWindow = new AddShopsWindow(_dm);
			adminWindow.Show();
			this.Close();
		}

        private void BtnManageThemes_Click(object sender, RoutedEventArgs e)
        {
            ManageThemesWindow window = new ManageThemesWindow(_dm);
            window.Show();
            this.Close();
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            StartWindow window = new StartWindow(_dm);
            window.Show();
            this.Close();
        }

    }
}
