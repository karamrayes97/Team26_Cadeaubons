using Cadeaubons_Domain;
using Cadeaubons_Domain.DTO;
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
	/// Interaction logic for AddShopsWindow.xaml
	/// </summary>
	public partial class AddShopsWindow : Window
	{
		private DomainManager _domainManger;
		public AddShopsWindow(DomainManager domainManager)
		{
			InitializeComponent();
			_domainManger = domainManager;
			CitiesListComboBox.ItemsSource = domainManager.GetCities();
			CitiesListComboBox.SelectedIndex = 0;
		}

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            AdminWindow window = new AdminWindow(_domainManger);
            window.Show();
            this.Close();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            StoreDTO storeDTO = new StoreDTO();
            storeDTO.Name = StoreNameTextBox.Text.Trim();
            storeDTO.Adress = StoreAdressTextBox.Text;
            storeDTO.PhoneNumber = StoreNumberTextBox.Text.Trim();
            storeDTO.City = (CityDTO)CitiesListComboBox.SelectedItem;

            foreach (PropertyInfo prop in storeDTO.GetType().GetProperties())
            {
                var value = prop.GetValue(storeDTO);

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
            }

            try
            {
                _domainManger.AddStore(storeDTO);
                MessageHelper.ShowInfo("Winkel succesvol toegevoegd!");
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError(ex.Message);
            }
        }

        private static string TranslateField(string propName) => propName switch
        {
            "Name" => "een naam",
            "Adress" => "een adres",
            "PhoneNumber" => "een telefoonnummer",
            "City" => "een stad",
            _ => propName
        };
    }


}
