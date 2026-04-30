using Cadeaubons_Domain.Model;
using System;

namespace Cadeaubons_Domain.DTO
{
    public sealed class ConsumptionDTO
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string Reason { get; set; }
        public DateTime Date { get; set; }
        public string StoreName { get; set; }

        public ConsumptionDTO() { }

        public ConsumptionDTO(Consumption c)
        {
            Id = c.Id;
            Amount = c.Amount;
            Reason = c.Reason;
            Date = c.Date;
            StoreName = c.Store?.Name ?? "-";
        }
    }
}