using Cadeaubons_Domain.DTO;
using Cadeaubons_Domain.Model;
using Cadeaubons_Domain.Repo;
using Cadeaubons_Domain.Security;
using Cadeaubons_Domain.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Xunit;

namespace Cadeaubons_XunitTesting
{
    public class VoucherServiceTests : IDisposable
    {
        private readonly Repository _repository;
        private readonly VoucherService _voucherService;

        // xUnit roept de constructor opnieuw aan voor elke test → schone DB per test
        public VoucherServiceTests()
        {
            var options = new DbContextOptionsBuilder<Repository>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _repository = new Repository(options);
            _voucherService = new VoucherService(_repository);
        }

        public void Dispose()
        {
            _repository.Dispose();
        }

        // ============ HELPERS ============

        private User CreateAndSaveUser(string email)
        {
            string salt = PasswordHelper.GenerateSalt();
            var user = new User
            {
                FirstName = "Test",
                LastName = "User",
                Email = email,
                PhoneNumber = "0470000000",
                DateOfBirth = new DateTime(1990, 1, 1),
                Role = Role.Customer,
                IsActive = true,
                PasswordSalt = salt,
                PasswordHash = PasswordHelper.HashPassword("test", salt)
            };
            _repository.Users.Add(user);
            _repository.SaveChanges();
            return user;
        }

        private Theme CreateAndSaveTheme(string name = "Verjaardag")
        {
            var theme = new Theme
            {
                Name = name,
                Description = "Test thema",
                IconPath = "🎂",
                PrimaryColor = "#FF6B6B"
            };
            _repository.Themes.Add(theme);
            _repository.SaveChanges();
            return theme;
        }

        private Store CreateAndSaveStore()
        {
            var city = new City { Name = "Gent", PostalCode = "9000" };
            _repository.Cities.Add(city);
            _repository.SaveChanges();

            var store = new Store
            {
                Name = "Test Winkel",
                Address = "Teststraat 1",
                PhoneNumber = "092224455",
                City = city
            };
            _repository.Stores.Add(store);
            _repository.SaveChanges();
            return store;
        }

        // ============ TESTS ============

        [Fact]
        public void AddVoucher_ValidData_SavesToDatabase()
        {
            // Arrange
            var buyer = CreateAndSaveUser("buyer@test.be");
            var theme = CreateAndSaveTheme();

            var dto = new VoucherDTO
            {
                BuyerId = buyer.Id,
                UserId = buyer.Id,
                ThemeId = theme.Id,
                InitialAmount = 50m,
                PurchaseDate = DateTime.Now
            };

            // Act
            Voucher result = _voucherService.AddVoucher(dto);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Number);
            Assert.True(VoucherNumberHelper.IsValidNumber(result.Number));
            Assert.Single(_repository.Vouchers);
            Assert.Equal(50m, _repository.Vouchers.First().InitialAmount);
        }

        [Fact]
        public void GetVouchersForUser_AsBuyer_ReturnsBoughtVoucher()
        {
            // Arrange
            var buyer = CreateAndSaveUser("buyer@test.be");
            var recipient = CreateAndSaveUser("recipient@test.be");
            var theme = CreateAndSaveTheme();

            _voucherService.AddVoucher(new VoucherDTO
            {
                BuyerId = buyer.Id,
                UserId = recipient.Id,
                ThemeId = theme.Id,
                InitialAmount = 25m,
                PurchaseDate = DateTime.Now
            });

            // Act
            var vouchers = _voucherService.GetVouchersForUser(buyer.Id);

            // Assert
            Assert.Single(vouchers);
            Assert.Equal(25m, vouchers[0].InitialAmount);
            Assert.Equal(25m, vouchers[0].RemainingAmount); // geen consumpties
        }

        [Fact]
        public void GetVouchersForUser_AsRecipient_ReturnsReceivedVoucher()
        {
            // Arrange
            var buyer = CreateAndSaveUser("buyer@test.be");
            var recipient = CreateAndSaveUser("recipient@test.be");
            var theme = CreateAndSaveTheme();

            _voucherService.AddVoucher(new VoucherDTO
            {
                BuyerId = buyer.Id,
                UserId = recipient.Id,
                ThemeId = theme.Id,
                InitialAmount = 100m,
                PurchaseDate = DateTime.Now
            });

            // Act
            var vouchers = _voucherService.GetVouchersForUser(recipient.Id);

            // Assert
            Assert.Single(vouchers);
            Assert.Equal(100m, vouchers[0].InitialAmount);
        }

        [Fact]
        public void GetVouchersForUser_FullyConsumed_IsExcluded()
        {
            // Arrange
            var buyer = CreateAndSaveUser("buyer@test.be");
            var theme = CreateAndSaveTheme();
            var store = CreateAndSaveStore();

            var voucher = _voucherService.AddVoucher(new VoucherDTO
            {
                BuyerId = buyer.Id,
                UserId = buyer.Id,
                ThemeId = theme.Id,
                InitialAmount = 30m,
                PurchaseDate = DateTime.Now
            });

            // Volledig opgebruikt: één consumptie ter waarde van het volledige bedrag
            _repository.Consumptions.Add(new Consumption
            {
                Amount = 30m,
                Reason = "Volledig gebruikt",
                Voucher = voucher,
                Store = store
            });
            _repository.SaveChanges();

            // Act
            var vouchers = _voucherService.GetVouchersForUser(buyer.Id);

            // Assert
            Assert.Empty(vouchers); // volledig opgebruikte bons worden verborgen
        }

        [Fact]
        public void GetVouchersForUser_WithPartialConsumption_ReturnsCorrectBalance()
        {
            // Arrange
            var buyer = CreateAndSaveUser("buyer@test.be");
            var theme = CreateAndSaveTheme();
            var store = CreateAndSaveStore();

            var voucher = _voucherService.AddVoucher(new VoucherDTO
            {
                BuyerId = buyer.Id,
                UserId = buyer.Id,
                ThemeId = theme.Id,
                InitialAmount = 50m,
                PurchaseDate = DateTime.Now
            });

            _repository.Consumptions.Add(new Consumption
            {
                Amount = 15m,
                Reason = "Boek",
                Voucher = voucher,
                Store = store
            });
            _repository.Consumptions.Add(new Consumption
            {
                Amount = 10m,
                Reason = "Koffie",
                Voucher = voucher,
                Store = store
            });
            _repository.SaveChanges();

            // Act
            var vouchers = _voucherService.GetVouchersForUser(buyer.Id);

            // Assert
            Assert.Single(vouchers);
            Assert.Equal(50m, vouchers[0].InitialAmount);
            Assert.Equal(25m, vouchers[0].RemainingAmount);   // 50 - 15 - 10
            Assert.Equal(2, vouchers[0].Consumptions.Count);
        }

        [Fact]
        public void GetVouchersForUser_OrderedByPurchaseDateDescending()
        {
            // Arrange
            var buyer = CreateAndSaveUser("buyer@test.be");
            var theme = CreateAndSaveTheme();

            _voucherService.AddVoucher(new VoucherDTO
            {
                BuyerId = buyer.Id,
                UserId = buyer.Id,
                ThemeId = theme.Id,
                InitialAmount = 25m,
                PurchaseDate = DateTime.Now.AddDays(-10) // oud
            });

            _voucherService.AddVoucher(new VoucherDTO
            {
                BuyerId = buyer.Id,
                UserId = buyer.Id,
                ThemeId = theme.Id,
                InitialAmount = 50m,
                PurchaseDate = DateTime.Now // nieuw
            });

            // Act
            var vouchers = _voucherService.GetVouchersForUser(buyer.Id);

            // Assert
            Assert.Equal(2, vouchers.Count);
            Assert.True(vouchers[0].PurchaseDate > vouchers[1].PurchaseDate);
        }
    }
}