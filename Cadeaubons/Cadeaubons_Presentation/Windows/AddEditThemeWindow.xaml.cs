using Cadeaubons_Domain;
using Cadeaubons_Domain.DTO;
using Cadeaubons_Presentation.Helpers;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Cadeaubons_Presentation.Windows
{
    public class ColorOption
    {
        public string Hex { get; set; } = string.Empty;
        public SolidColorBrush Brush => new(
            (Color)ColorConverter.ConvertFromString(Hex));
    }

    public class EmojiOption
    {
        public string Emoji { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;
    }

    public partial class AddEditThemeWindow : Window
    {
        private readonly DomainManager _dm;
        private readonly ThemeDTO? _existingTheme;
        private string _selectedColor = string.Empty;
        private string _selectedEmoji = string.Empty;

        // Constructor for adding a new theme
        public AddEditThemeWindow(DomainManager dm)
        {
            InitializeComponent();
            _dm = dm;
            _existingTheme = null;
            TxtTitle.Text = "Add New Theme";
            BtnSave.Content = "Add Theme";
            LoadEmojiPalette();
            LoadColorPalette();
        }

        // Constructor for editing an existing theme
        public AddEditThemeWindow(DomainManager dm, ThemeDTO theme)
        {
            InitializeComponent();
            _dm = dm;
            _existingTheme = theme;
            TxtTitle.Text = "Edit Theme";
            BtnSave.Content = "Save Changes";

            TxtName.Text = theme.Name;
            TxtDescription.Text = theme.Description;

            LoadEmojiPalette();
            LoadColorPalette();
            SelectEmoji(theme.IconPath);
            SelectColor(theme.PrimaryColor);
        }

        private void LoadEmojiPalette()
        {
            List<EmojiOption> emojis = new()
            {
                // Feest & Vieringen
                new() { Emoji = "🎁", Label = "Cadeau" },
                new() { Emoji = "🎂", Label = "Verjaardag" },
                new() { Emoji = "🎉", Label = "Feest" },
                new() { Emoji = "🎊", Label = "Confetti" },
                new() { Emoji = "🎈", Label = "Ballon" },
                new() { Emoji = "🎄", Label = "Kerst" },
                new() { Emoji = "🎃", Label = "Halloween" },
                new() { Emoji = "🎆", Label = "Vuurwerk" },

                // Liefde & Relaties
                new() { Emoji = "❤️", Label = "Hart" },
                new() { Emoji = "💍", Label = "Huwelijk" },
                new() { Emoji = "💐", Label = "Boeket" },
                new() { Emoji = "🌹", Label = "Roos" },
                new() { Emoji = "💝", Label = "Hartje cadeau" },
                new() { Emoji = "💕", Label = "Hartjes" },
                new() { Emoji = "👶", Label = "Baby" },
                new() { Emoji = "🤰", Label = "Zwanger" },

                // School & Werk
                new() { Emoji = "🎓", Label = "Geslaagd" },
                new() { Emoji = "📚", Label = "Boeken" },
                new() { Emoji = "🏆", Label = "Trofee" },
                new() { Emoji = "⭐", Label = "Ster" },
                new() { Emoji = "🙏", Label = "Bedankt" },
                new() { Emoji = "👏", Label = "Applaus" },
                new() { Emoji = "💼", Label = "Werk" },
                new() { Emoji = "🎯", Label = "Doel" },

                // Eten & Drinken
                new() { Emoji = "🍽️", Label = "Restaurant" },
                new() { Emoji = "☕", Label = "Koffie" },
                new() { Emoji = "🍕", Label = "Pizza" },
                new() { Emoji = "🍷", Label = "Wijn" },
                new() { Emoji = "🧁", Label = "Cupcake" },
                new() { Emoji = "🍫", Label = "Chocolade" },
                new() { Emoji = "🎂", Label = "Taart" },
                new() { Emoji = "🍾", Label = "Champagne" },

                // Vrije Tijd & Reizen
                new() { Emoji = "✈️", Label = "Reizen" },
                new() { Emoji = "🏖️", Label = "Strand" },
                new() { Emoji = "🧖", Label = "Wellness" },
                new() { Emoji = "🎵", Label = "Muziek" },
                new() { Emoji = "🎮", Label = "Gaming" },
                new() { Emoji = "⚽", Label = "Sport" },
                new() { Emoji = "🎬", Label = "Film" },
                new() { Emoji = "📷", Label = "Foto" },

                // Natuur & Seizoenen
                new() { Emoji = "🌸", Label = "Lente" },
                new() { Emoji = "☀️", Label = "Zomer" },
                new() { Emoji = "🍂", Label = "Herfst" },
                new() { Emoji = "❄️", Label = "Winter" },
                new() { Emoji = "🌈", Label = "Regenboog" },
                new() { Emoji = "🌻", Label = "Zonnebloem" },
                new() { Emoji = "🦋", Label = "Vlinder" },
                new() { Emoji = "🐾", Label = "Huisdier" },
            };

            EmojiPalette.ItemsSource = emojis;
        }

        private void LoadColorPalette()
        {
            List<ColorOption> colors = new()
            {
                // Reds
                new() { Hex = "#FF0000" },
                new() { Hex = "#CC0000" },
                new() { Hex = "#990000" },
                new() { Hex = "#FF4444" },
                new() { Hex = "#FF6666" },
                new() { Hex = "#FFCCCC" },

                // Oranges
                new() { Hex = "#FF8800" },
                new() { Hex = "#FF6600" },
                new() { Hex = "#CC5500" },
                new() { Hex = "#FFAA44" },
                new() { Hex = "#FFCC88" },
                new() { Hex = "#FFE0B2" },

                // Yellows
                new() { Hex = "#FFCC00" },
                new() { Hex = "#FFD700" },
                new() { Hex = "#FFF176" },
                new() { Hex = "#FFEB3B" },
                new() { Hex = "#FFF9C4" },
                new() { Hex = "#FFFDE7" },

                // Greens
                new() { Hex = "#00AA00" },
                new() { Hex = "#2E7D32" },
                new() { Hex = "#4CAF50" },
                new() { Hex = "#66BB6A" },
                new() { Hex = "#A5D6A7" },
                new() { Hex = "#C8E6C9" },

                // Teals
                new() { Hex = "#009688" },
                new() { Hex = "#00897B" },
                new() { Hex = "#26A69A" },
                new() { Hex = "#4DB6AC" },
                new() { Hex = "#80CBC4" },
                new() { Hex = "#B2DFDB" },

                // Blues
                new() { Hex = "#0066CC" },
                new() { Hex = "#1565C0" },
                new() { Hex = "#1E88E5" },
                new() { Hex = "#42A5F5" },
                new() { Hex = "#90CAF9" },
                new() { Hex = "#BBDEFB" },

                // Purples
                new() { Hex = "#7B1FA2" },
                new() { Hex = "#9C27B0" },
                new() { Hex = "#AB47BC" },
                new() { Hex = "#CE93D8" },
                new() { Hex = "#E1BEE7" },
                new() { Hex = "#F3E5F5" },

                // Pinks
                new() { Hex = "#C2185B" },
                new() { Hex = "#E91E63" },
                new() { Hex = "#EC407A" },
                new() { Hex = "#F48FB1" },
                new() { Hex = "#F8BBD0" },
                new() { Hex = "#FCE4EC" },

                // Neutrals
                new() { Hex = "#000000" },
                new() { Hex = "#424242" },
                new() { Hex = "#757575" },
                new() { Hex = "#9E9E9E" },
                new() { Hex = "#BDBDBD" },
                new() { Hex = "#FFFFFF" },
            };

            ColorPalette.ItemsSource = colors;
        }

        private void Emoji_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is System.Windows.Controls.Border border && border.Tag is string emoji)
            {
                SelectEmoji(emoji);
            }
        }

        private void SelectEmoji(string emoji)
        {
            _selectedEmoji = emoji;
            TxtSelectedEmoji.Text = emoji;
            var options = EmojiPalette.ItemsSource as List<EmojiOption>;
            var match = options?.Find(o => o.Emoji == emoji);
            TxtSelectedEmojiLabel.Text = match?.Label ?? emoji;
            TxtSelectedEmojiLabel.Foreground = new SolidColorBrush(Colors.Black);
        }

        private void Color_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is System.Windows.Controls.Border border && border.Tag is string hex)
            {
                SelectColor(hex);
            }
        }

        private void SelectColor(string hex)
        {
            _selectedColor = hex;
            try
            {
                var color = (Color)ColorConverter.ConvertFromString(hex);
                ColorPreview.Background = new SolidColorBrush(color);
                TxtSelectedColor.Text = hex;
                TxtSelectedColor.Foreground = new SolidColorBrush(Colors.Black);
            }
            catch
            {
                ColorPreview.Background = new SolidColorBrush(Colors.LightGray);
                TxtSelectedColor.Text = "Invalid color";
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            string name = TxtName.Text.Trim();
            string description = TxtDescription.Text.Trim();
            string iconPath = _selectedEmoji;

            if (string.IsNullOrWhiteSpace(name))
            {
                MessageHelper.ShowWarning("Please enter a theme name.");
                return;
            }

            if (string.IsNullOrWhiteSpace(iconPath))
            {
                MessageHelper.ShowWarning("Please select an icon.");
                return;
            }

            if (string.IsNullOrWhiteSpace(_selectedColor))
            {
                MessageHelper.ShowWarning("Please select a color.");
                return;
            }

            try
            {
                if (_existingTheme == null)
                {
                    _dm.AddTheme(name, description, iconPath, _selectedColor);
                    MessageHelper.ShowInfo("Theme added successfully!");
                }
                else
                {
                    _dm.UpdateTheme(_existingTheme.Id, name, description,
                        iconPath, _selectedColor);
                    MessageHelper.ShowInfo("Theme updated successfully!");
                }

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

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}