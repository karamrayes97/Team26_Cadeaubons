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
using Cadeaubons_Domain.Model;

namespace Cadeaubons_Presentation.Windows
{
    /// <summary>
    /// Interaction logic for ThemeListWindow.xaml
    /// </summary>
    public partial class ThemeListWindow : Window
    {
        private readonly DomainManager _dm;
        private UserDTO _currentUser;
        private List<ThemeDTO> _themes;
        
        public ThemeListWindow(DomainManager dm, UserDTO currentUser)
        {
            
            InitializeComponent();
            _dm = dm;
            _currentUser = currentUser;
            LoadThemes();
        }


        private void LoadThemes()
        {
            _themes = _dm.GetAllThemes();

            ThemeList.ItemsSource = _themes;
        }

        private void ThemeList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ThemeList.SelectedItem is ThemeDTO selectedTheme)
            {
                ThemeDetailWindow window = new ThemeDetailWindow(_dm, _currentUser, selectedTheme);
                window.Show();
                this.Close();
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            CustomerWindow customerWindow = new CustomerWindow(_dm, _currentUser);
            _currentUser = null;
            customerWindow.Show();
            this.Close();
        }
    }
}
