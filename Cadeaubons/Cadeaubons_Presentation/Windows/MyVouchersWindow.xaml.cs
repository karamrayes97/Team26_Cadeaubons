using System.Windows;
using System.Windows.Controls;
using Cadeaubons_Domain;
using Cadeaubons_Domain.DTO;

namespace Cadeaubons_Presentation.Windows
{
    public partial class MyVouchersWindow : Window
    {
        private readonly DomainManager _dm;
        private readonly UserDTO _currentUser;

        public MyVouchersWindow(DomainManager dm, UserDTO currentUser)
        {
            InitializeComponent();
            _dm = dm;
            _currentUser = currentUser;
            LoadVouchers();
        }

        private void LoadVouchers()
        {
            var vouchers = _dm.GetVouchersForUser(_currentUser.Id);
            VouchersGrid.ItemsSource = vouchers;
        }

        private void VouchersGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (VouchersGrid.SelectedItem is VoucherOverviewDTO selected)
            {
                ConsumptionsGrid.ItemsSource = selected.Consumptions;
                DetailsBox.Visibility = selected.Consumptions.Count > 0
                    ? Visibility.Visible
                    : Visibility.Collapsed;

                BtnConsume.IsEnabled = !selected.IsExpired && selected.RemainingAmount > 0;
            }
            else
            {
                DetailsBox.Visibility = Visibility.Collapsed;
                BtnConsume.IsEnabled = false;
            }
        }

        private void BtnConsume_Click(object sender, RoutedEventArgs e)
        {
            var selected = (VoucherOverviewDTO) VouchersGrid.SelectedItem;
            var window = new ConsumeVoucherWindow(_dm, _currentUser, selected);
            window.Show();
            this.Close();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            CustomerWindow window = new CustomerWindow(_dm, _currentUser);
            window.Show();
            this.Close();
        }
    }
}