using Cadeaubons_API;
using Cadeaubons_Domain;
using Cadeaubons_Domain.DTO;
using Cadeaubons_Domain.Model;
using Cadeaubons_Presentation.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Threading.Tasks;


namespace Cadeaubons_Presentation.Windows
{


	public class CheckoutResponse
	{
		public string url { get; set; }
		public string sessionId { get; set; }
	}
	/// <summary>
	/// Interaction logic for BuyGiftCardWindow.xaml
	/// </summary>
	public partial class BuyGiftCardWindow : Window
	{
		private DomainManager _domainManager;
        private readonly UserDTO _currentUser;

        public BuyGiftCardWindow(DomainManager domainManger, UserDTO currentUser)
		{
			InitializeComponent();
			_domainManager = domainManger;
            _currentUser = currentUser;
            ThemeComboBox.ItemsSource = _domainManager.GetAllThemes();
			ThemeComboBox.SelectedIndex = 0;
            ApplyThemeColor();
        }

		private void Button_Click(object sender, RoutedEventArgs e) //buy for you self
		{
			EmailTextBox.Text = "";
			EmailTextBox.Text = _currentUser.Email;


		}

        private void ThemeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyThemeColor();
        }

        private void ApplyThemeColor()
        {
            if (ThemeComboBox.SelectedItem is not ThemeDTO theme) return;

            try
            {
                var color = (Color)ColorConverter.ConvertFromString(theme.PrimaryColor);
                ThemeComboBox.Background = new SolidColorBrush(color);
                ThemeComboBox.Foreground = new SolidColorBrush(GetContrastingForeground(color));
            }
            catch
            {
                // Ongeldige hex-waarde — laat de standaardkleur staan
            }
        }

        // Helper: kies wit of zwart op basis van helderheid van de achtergrondkleur
        private static Color GetContrastingForeground(Color bg)
        {
            // YIQ formule (perceived brightness)
            double brightness = (bg.R * 299 + bg.G * 587 + bg.B * 114) / 1000.0;
            return brightness > 128 ? Colors.Black : Colors.White;
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            CustomerWindow window = new CustomerWindow(_domainManager, _currentUser);
            window.Show();
            this.Close();
        }

        public async Task StartStripeAsync(VoucherDTO dTO)
		{
			using var client = new HttpClient();

			////var response = await client.PostAsync(
			////	"https://localhost:7011/create-checkout-session",
			////	null
			////);

			var response = await client.PostAsJsonAsync(
				"https://localhost:7011/create-checkout-session",
				dTO
			);

			response.EnsureSuccessStatusCode();

			var json = await response.Content.ReadAsStringAsync();
			var data = JsonSerializer.Deserialize<CheckoutResponse>(json);



			Process.Start(new ProcessStartInfo
			{
				FileName = data.url,
				UseShellExecute = true

			});
		}

        private async void ButtonBuy_Click(object sender, RoutedEventArgs e)
        {
            VoucherDTO voucherDTO = new VoucherDTO();
            voucherDTO.PurchaseDate = DateTime.Now;

            string input = InitialAmountTextBox.Text.Trim().Replace('.', ',');
            bool resultBool = decimal.TryParse(input, out decimal result);
            if (resultBool)
            {
                voucherDTO.InitialAmount = result;
            }

            voucherDTO.BuyerId = _currentUser.Id;
            voucherDTO.BuyerFullName = $"{_currentUser.FirstName} {_currentUser.LastName}";

            if (!string.IsNullOrEmpty(EmailTextBox.Text))
            {
                UserDTO userDTO = _domainManager.GetByEmail(EmailTextBox.Text);
                if (userDTO != null)
                {
                    voucherDTO.UserId = userDTO.Id;
                    voucherDTO.UserEmail = userDTO.Email;
                    voucherDTO.UserFullName = $"{userDTO.FirstName} {userDTO.LastName}";
                }
                else
                {
                    MessageHelper.ShowWarning("E-mailadres ontvanger is niet gekoppeld aan een actieve gebruiker.");
                    return;
                }
            }

            ThemeDTO themeDTO = (ThemeDTO)ThemeComboBox.SelectedItem;
            voucherDTO.ThemeId = themeDTO.Id;
            voucherDTO.ThemeName = themeDTO.Name;

            // Validatie van DTO-velden
            foreach (PropertyInfo prop in voucherDTO.GetType().GetProperties())
            {
                var value = prop.GetValue(voucherDTO);
                if (value == null)
                {
                    MessageHelper.ShowWarning($"Vul {TranslateField(prop.Name)} in.");
                    return;
                }

                if (value is string str && string.IsNullOrWhiteSpace(str))
                {
                    MessageHelper.ShowWarning($"Vul {TranslateField(prop.Name)} in.");
                    return;
                }

                if (value is int intValue && intValue == 0)
                {
                    if (prop.Name == "UserId")
                        MessageHelper.ShowWarning("Voer het e-mailadres van de ontvanger in.");
                    else
                        MessageHelper.ShowWarning($"Vul {TranslateField(prop.Name)} in.");
                    return;
                }

                if (value is decimal decValue && decValue == 0)
                {
                    MessageHelper.ShowWarning($"Vul {TranslateField(prop.Name)} in.");
                    return;
                }
            }

            // Start de betaalflow
            try
            {
                // Onthoud hoeveel bons de gebruiker nu heeft
                int countBefore = _domainManager.GetVouchersForUser(_currentUser.Id).Count;

                // Toon "bezig" status op de knop
                ButtonBuy.IsEnabled = false;
                string originalContent = ButtonBuy.Content.ToString();
                ButtonBuy.Content = "⏳ Betaling wordt verwerkt...";

                await StartStripeAsync(voucherDTO);  //ngrok http https://localhost:7011

                // Wacht tot de webhook een nieuwe bon heeft toegevoegd (max 60 seconden)
                bool success = await WaitForVoucherAsync(countBefore, timeoutSeconds: 60);

                // Herstel de knop (voor het geval het venster nog open blijft bij fout)
                ButtonBuy.Content = originalContent;
                ButtonBuy.IsEnabled = true;

                PaymentResultWindow resultWindow =
                    new PaymentResultWindow(_domainManager, _currentUser, success);
                resultWindow.Show();
                this.Close();
            }
            catch (Exception)
            {
                // Stripe niet bereikbaar of andere onverwachte fout
                PaymentResultWindow resultWindow =
                    new PaymentResultWindow(_domainManager, _currentUser, false);
                resultWindow.Show();
                this.Close();
            }
        }


        //ngrok http https://localhost:7011
        private static string TranslateField(string propName) => propName switch
        {
            "InitialAmount" => "een bedrag",
            "PurchaseDate" => "een aankoopdatum",
            "BuyerId" => "de koper",
            "UserId" => "het e-mailadres van de ontvanger",
            "ThemeId" => "een thema",
            _ => propName
        };

        
        /// Pollt de databank tot er een nieuwe bon verschijnt voor de huidige gebruiker,
        /// of tot de timeout verstrijkt. Geeft true terug als er een nieuwe bon is gedetecteerd.
       
        private async Task<bool> WaitForVoucherAsync(int countBefore, int timeoutSeconds)
        {
            DateTime deadline = DateTime.Now.AddSeconds(timeoutSeconds);

            while (DateTime.Now < deadline)
            {
                int countNow = _domainManager.GetVouchersForUser(_currentUser.Id).Count;
                if (countNow > countBefore)
                    return true;

                await Task.Delay(1000); // check elke seconde
            }

            return false;
        }


    }


}
