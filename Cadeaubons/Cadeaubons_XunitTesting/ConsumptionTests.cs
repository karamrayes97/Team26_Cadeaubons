using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cadeaubons_Domain;
using Cadeaubons_Domain.DTO;
using Cadeaubons_Domain.Model;
using Cadeaubons_Domain.Repo;
using Cadeaubons_Domain.Services;
using Microsoft.EntityFrameworkCore;

namespace Cadeaubons_XunitTesting
{
    public class ConsumptionTests
    {
        private Repository GetDbContext()
        {
            var options = new DbContextOptionsBuilder<Repository>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new Repository(options);

            TestDataSeeder.Seed(context);

            return context;
        }

        [Fact]
        public void Add_ShouldPersistConsumptionAndReturnDTO()
        {
            // Arrange
            var db = GetDbContext();

            var service = new ConsumptionService(db);

            var voucher = db.Vouchers.First();
            var store = db.Stores.First();

            var dto = new ConsumptionDTO
            {
                Amount = 25,
                Reason = "Lunch",
                VoucherId = voucher.Id,
                StoreId = store.Id
            };

            // Act
            var result = service.Add(dto, 50, false);

            // Assert DTO
            Assert.NotNull(result);
            Assert.Equal(25, result.Amount);
            Assert.Equal("Lunch", result.Reason);
            Assert.Equal(voucher.Id, result.VoucherId);
            Assert.Equal(store.Id, result.StoreId);

            // Assert DB
            var saved = db.Consumptions.FirstOrDefault(c =>
                c.VoucherId == voucher.Id &&
                c.StoreId == store.Id);

            Assert.NotNull(saved);
            Assert.Equal(25, saved.Amount);
            Assert.Equal("Lunch", saved.Reason);
        }

        [Fact]
        public void AddConsumption_ShouldThrow_WhenVoucherExpired()
        {
            var db = GetDbContext();

            var voucher = db.Vouchers.First();

            // force expiration
            var voucherDTO = new VoucherOverviewDTO(voucher, db.Consumptions)
            {
                ExpiryDate = DateTime.Now.AddDays(-1) // expired
            };

            var consumptionDTO = new ConsumptionDTO
            {
                Amount = 10,
                Reason = "Test",
                VoucherId = voucher.Id,
                StoreId = db.Stores.First().Id
            };

            var service = new ConsumptionService(db);

            Assert.Throws<InvalidOperationException>(() =>
                service.Add(consumptionDTO, voucherDTO.RemainingAmount, voucherDTO.IsExpired));
        }

        [Fact]
        public void AddConsumption_ShouldThrow_WhenInsufficientBalance()
        {
            var db = GetDbContext();

            var voucher = db.Vouchers.First();

            var voucherDTO = new VoucherOverviewDTO(voucher, db.Consumptions);

            var consumptionDTO = new ConsumptionDTO
            {
                Amount = voucherDTO.RemainingAmount + 100, // too high
                Reason = "Test",
                VoucherId = voucher.Id,
                StoreId = db.Stores.First().Id
            };

            var service = new ConsumptionService(db);

            Assert.Throws<InvalidOperationException>(() =>
                service.Add(consumptionDTO, voucherDTO.RemainingAmount, voucherDTO.IsExpired));
        }

    }
}
