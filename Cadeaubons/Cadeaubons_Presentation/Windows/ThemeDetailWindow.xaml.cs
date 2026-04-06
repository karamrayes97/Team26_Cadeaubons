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
using Cadeaubons_Domain;
using Cadeaubons_Domain.DTO;
using Cadeaubons_Domain.Model;

namespace Cadeaubons_Presentation.Windows
{
    /// <summary>
    /// Interaction logic for ThemeDetailWindow.xaml
    /// </summary>
    public partial class ThemeDetailWindow : Window
    {
        private readonly DomainManager _dm;
        private UserDTO _currentUser;
        private readonly ThemeDTO _theme;

        public ThemeDetailWindow(DomainManager dm, UserDTO currentUser, ThemeDTO theme)
        {
            InitializeComponent();
            _dm = dm;
            _currentUser = currentUser;
            _theme = theme;
            LoadTheme();
            
        }

        private void LoadTheme()
        {
            ThemeName.Text = _theme.Name;
            ThemeDescription.Text = _theme.Description;

            ThemeIcon.Source = new BitmapImage(
                new Uri(_theme.IconPath, UriKind.RelativeOrAbsolute)
            );

            var color = (Color)ColorConverter.ConvertFromString(_theme.PrimaryColor);

            ColorPreview.Background = new SolidColorBrush(color);

            ColorCodeText.Text = $"Color code: {_theme.PrimaryColor}";
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            ThemeListWindow window = new ThemeListWindow(_dm, _currentUser);
            _currentUser = null;
            window.Show();
            this.Close();
        }
    }
}
