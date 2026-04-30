using Cadeaubons_Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cadeaubons_Domain.DTO
{

    
    public sealed class VoucherOverviewDTO
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public decimal InitialAmount { get; set; }
        public decimal RemainingAmount { get; set; }
        public DateTime PurchaseDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string ThemeName { get; set; }
        public string BuyerName { get; set; }
        public string ReceiverName { get; set; }
        public List<ConsumptionDTO> Consumptions { get; set; } = new();

        
        public bool IsExpired => DateTime.Now > ExpiryDate;
        public int DaysRemaining => Math.Max(0, (ExpiryDate - DateTime.Now).Days);
        public string ValidityText => IsExpired
            ? "Verlopen"
            : $"Nog {DaysRemaining} dagen geldig (tot {ExpiryDate:dd/MM/yyyy})";

        public VoucherOverviewDTO() { }

        public VoucherOverviewDTO(Voucher voucher, IEnumerable<Consumption> consumptions)
        {
            Id = voucher.Id;
            Number = voucher.Number;
            InitialAmount = voucher.InitialAmount;
            PurchaseDate = voucher.PurchaseDate;
            ExpiryDate = voucher.PurchaseDate.AddYears(1); // default 1 jaar geldig
            ThemeName = voucher.Theme?.Name ?? "-";
            BuyerName = voucher.Buyer != null
                ? $"{voucher.Buyer.FirstName} {voucher.Buyer.LastName}"
                : "-";
            ReceiverName = voucher.User != null
                ? $"{voucher.User.FirstName} {voucher.User.LastName}"
                : "-";

            Consumptions = consumptions
                .Select(c => new ConsumptionDTO(c))
                .OrderByDescending(c => c.Date)
                .ToList();

            decimal totalUsed = Consumptions.Sum(c => c.Amount);
            RemainingAmount = InitialAmount - totalUsed;
        }
    }
}