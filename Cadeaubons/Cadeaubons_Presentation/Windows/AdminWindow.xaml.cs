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
	}
}
