using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadeaubons_Domain.Model
{
	[Table("Payments")]
	public class Payment
	{
        private int _id;
        [Key]
        [Column("Id")]
        public int Id
        {
            get => _id;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Id must be zero or positive.", nameof(Id));

                _id = value;
            }
        }

        private decimal _amount;
        [Column("Amount", TypeName = "decimal(10,2)")]
        public decimal Amount
        {
            get => _amount;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Amount must be positive.", nameof(Amount));

                _amount = value;
            }
        }

        [Column("Date")]
		public DateTime Date { get; set; } = DateTime.Now;

        [Column("Status")]
		public PaymentStatus Status { get; set; }

        private string _stripePaymentId = string.Empty;
        [Column("StripePaymentId"), MaxLength(100)]
		public string StripePaymentId
        {
            get => _stripePaymentId;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Stripe payment id is required.", nameof(StripePaymentId));

                _stripePaymentId = value.Trim();
            }
        }

        [ForeignKey("Voucher")]
        [Column("VoucherId")]
        public int VoucherId { get; set; }

        private Voucher _voucher;
        public Voucher Voucher
        {
            get => _voucher;
            set
            {
                if (value == null)
                    throw new ArgumentException("Voucher is required.", nameof(Voucher));

                _voucher = value;
                VoucherId = value.Id;
            }
        }

        public override bool Equals(object? obj)
        {
            return obj is Payment payment &&
                string.Equals(payment.StripePaymentId, StripePaymentId, StringComparison.OrdinalIgnoreCase);
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(StringComparer.OrdinalIgnoreCase.GetHashCode(StripePaymentId ?? string.Empty));
        }

        public override string ToString()
        {
            return $"Payment - Id: {Id}, Amount: {Amount:C}, Date: {Date:yyyy-MM-dd HH:mm}, Voucher: {Voucher?.Number}, Stripe payment id: {StripePaymentId}";
        }
    }
}
