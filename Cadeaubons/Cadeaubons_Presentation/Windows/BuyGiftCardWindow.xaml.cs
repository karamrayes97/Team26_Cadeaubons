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
		public BuyGiftCardWindow(DomainManager domainManger)
		{
			InitializeComponent();
			_domainManager = domainManger;
			ThemeComboBox.ItemsSource = _domainManager.GetAllThemes();
			ThemeComboBox.SelectedIndex = 0;
		}

		private void Button_Click(object sender, RoutedEventArgs e) //buy for you self
		{
			EmailTextBox.Text = "";
			EmailTextBox.Text = Session.CurrentUser.Email;


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

			bool resultBool = int.TryParse(InitialAmountTextBox.Text, out int result);
			if (resultBool)
			{
				voucherDTO.InitialAmount = result;
			}

			voucherDTO.BuyerId = Session.CurrentUser.Id;

			if (!string.IsNullOrEmpty(EmailTextBox.Text))
			{
				UserDTO userDTO = _domainManager.GetByEmail(EmailTextBox.Text);
				if (userDTO != null)
				{
					voucherDTO.UserId = userDTO.Id;
				}
				else
				{
					MessageHelper.ShowWarning($"Recipient email is not linked to active user");
					return;
				}

			}


			ThemeDTO themeDTO = (ThemeDTO)ThemeComboBox.SelectedItem;
			voucherDTO.ThemeId = themeDTO.Id;


			foreach (PropertyInfo prop in voucherDTO.GetType().GetProperties())
			{
				var value = prop.GetValue(voucherDTO);
				if (value == null)
				{
					MessageHelper.ShowWarning($"Please enter {prop.Name}.");
					return;
				}


				if (value is string str && string.IsNullOrWhiteSpace(str))
				{
					MessageHelper.ShowWarning($"Please enter {prop.Name}.");
					return;
				}

				if (value is int intValue)
				{
					if (intValue == 0)
					{
						if (prop.Name == "UserId")
						{
							MessageHelper.ShowWarning($"Enter Recipient Email Adress");
						}
						else
						{
							MessageHelper.ShowWarning($"{prop.Name} cannot be 0.");
						}

						return;
					}

				}

			}
			//ngrok http https://localhost:7011
			try
			{
				await StartStripeAsync(voucherDTO);
				MessageHelper.ShowInfo("Redirecting to payment...");
			}
			catch (Exception ex)
			{

				MessageHelper.ShowError(ex.Message);
			}





		}


	}
}
