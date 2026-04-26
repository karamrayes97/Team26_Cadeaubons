using Cadeaubons_Domain;
using Cadeaubons_Domain.DTO;
using Cadeaubons_Domain.Model;
using Cadeaubons_Presentation.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

		private void ButtonBuy_Click(object sender, RoutedEventArgs e)
		{
			VoucherDTO voucherDTO = new VoucherDTO();
			voucherDTO.PurchaseDate = DateTime.Now;
			
			bool resultBool =  int.TryParse(InitialAmountTextBox.Text,out int result);
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
			try
			{
				_domainManager.AddVoucher(voucherDTO);
				MessageHelper.ShowInfo("Registration successful!");
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
	}
}
