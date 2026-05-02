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
using Cadeaubons_Presentation.Helpers;

namespace Cadeaubons_Presentation.Windows
{
    /// <summary>
    /// Interaction logic for ConsumeVoucherWindow.xaml
    /// </summary>
    public partial class ConsumeVoucherWindow : Window
    {
        private readonly DomainManager _dm;
        private readonly UserDTO _currentUser;
        private readonly VoucherOverviewDTO _voucherOverviewDTO;

        public ConsumeVoucherWindow(DomainManager dm, UserDTO currentUser, VoucherOverviewDTO voucherOverviewDTO)
        {
            InitializeComponent();

            _dm = dm;
            _currentUser = currentUser;
            _voucherOverviewDTO = voucherOverviewDTO;

            LoadData();
        }

        private void LoadData()
        {
            // Show remaining balance
            TxtBalance.Text = $"Beschikbaar saldo: {_voucherOverviewDTO.RemainingAmount:C}";

            // Load stores
            var stores = _dm.GetAllStores();
            CbStores.ItemsSource = stores;
            CbStores.DisplayMemberPath = "Name";
        }

        private void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (!decimal.TryParse(TxtAmount.Text, out decimal amount))
            {
                MessageHelper.ShowWarning("Please enter a valid amount.");
                return;
            }

            if (string.IsNullOrWhiteSpace(TxtReason.Text))
            {
                MessageHelper.ShowWarning("Please enter a reason.");
                return;
            }

            if (CbStores.SelectedItem is not StoreDTO selectedStore)
            {
                MessageHelper.ShowWarning("Please choose a store.");
                return;
            }

            ConsumptionDTO consumption = new ConsumptionDTO()
            {
                Amount = amount,
                Reason = TxtReason.Text,
                StoreId = selectedStore.Id,
                VoucherId = _voucherOverviewDTO.Id
            };

            try
            {
                _dm.AddConsumption( _voucherOverviewDTO, consumption);

                MessageHelper.ShowInfo("Consumption successfully registered.");
                OpenMyVouchersWindow();
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError(ex.Message);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            OpenMyVouchersWindow();
        }

        private void OpenMyVouchersWindow()
        {
            MyVouchersWindow window = new MyVouchersWindow(_dm, _currentUser);
            window.Show();
            this.Close();
        }
    }
}
