using Cadeaubons_Domain;
using Cadeaubons_Domain.DTO;
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
    public partial class ManageThemesWindow : Window
    {
        private readonly DomainManager _dm;

        public ManageThemesWindow(DomainManager dm)
        {
            InitializeComponent();
            _dm = dm;
            LoadThemes();
        }

        private void LoadThemes()
        {
            ThemeList.ItemsSource = _dm.GetAllThemes();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddEditThemeWindow window = new AddEditThemeWindow(_dm);
            window.Owner = this;
            if (window.ShowDialog() == true)
            {
                LoadThemes(); // refresh the list
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            int themeId = (int)((Button)sender).Tag;
            List<ThemeDTO> themes = _dm.GetAllThemes();
            ThemeDTO? theme = themes.Find(t => t.Id == themeId);

            if (theme == null) return;

            AddEditThemeWindow window = new AddEditThemeWindow(_dm, theme);
            window.Owner = this;
            if (window.ShowDialog() == true)
            {
                LoadThemes();
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            int themeId = (int)((Button)sender).Tag;

            MessageBoxResult result = MessageBox.Show(
                "Ben je zeker dat je dit thema wil verwijderen?",
                "Verwijderen bevestigen",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    _dm.DeleteTheme(themeId);
                    LoadThemes();
                    MessageHelper.ShowInfo("Thema succesvol verwijderd.");
                }
                catch (Exception ex)
                {
                    MessageHelper.ShowError(ex.Message);
                }
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            AdminWindow window = new AdminWindow(_dm);
            window.Show();
            this.Close();
        }
    }
}
