using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Cadeaubons_Domain.Model
{
	[Table("Consumptions")]
	public class Consumption
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
			set { 
				if (value <= 0)
					throw new ArgumentException("Amount must be positive.", nameof(Amount));

				_amount = value;
            } 
		}

        private string _reason = string.Empty;
        [Column("Reason")]
		public string Reason
        {
            get => _reason;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Reason is required.", nameof(Reason));

                _reason = value.Trim();
            }
        }

        [Column("Date")]
		public DateTime Date { get; } = DateTime.Now;

        [ForeignKey("Voucher")]
		[Column("VoucherId")]
		public int VoucherId { get; private set; }

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

        [ForeignKey("Store")]
		[Column("StoreId")]
		public int StoreId { get; private set; }

        private Store _store;
		public Store Store
        {
            get => _store;
            set
            {
                if (value == null)
                    throw new ArgumentException("Store is required.", nameof(Store));

                _store = value;
                StoreId = value.Id;
            }
        }

        public override string ToString()
        {
            return $"Consumption - Id: {Id}, Amount: {Amount:C}, Date: {Date:yyyy-MM-dd HH:mm}, Voucher: {Voucher?.Number}, Store: {Store?.Name}\nReason: {Reason}";
        }
    }
}
