using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cadeaubons_Domain.Services;

namespace Cadeaubons_Domain.Model
{
	[Table("Vouchers")]
	public class Voucher
	{
        private const int MINIMUM_AMOUNT = 20;
		private const int MAXIMUM_AMOUNT = 500;

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

        private string _number = string.Empty;

        [Column("Number")]
        public string Number
        {
            get => _number;
            init
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Number is required.", nameof(Number));

                value = value.Trim();

                if (!VoucherNumberHelper.IsValidNumber(value))
                    throw new ArgumentException("Invalid voucher number format or check digit.", nameof(Number));

                _number = value;
            }
        }

        private decimal _initialAmount;
		[Column("InitialAmount", TypeName = "decimal(10,2)")]
		public decimal InitialAmount 
		{ 
			get => _initialAmount;
			set { 
				if (value < MINIMUM_AMOUNT || value > MAXIMUM_AMOUNT)
					throw new ArgumentException($"InitialAmount must be between {MINIMUM_AMOUNT} and { MAXIMUM_AMOUNT}", nameof(InitialAmount));

				_initialAmount = value;
			}
		}

		[Column("PurchaseDate")]
		public DateTime PurchaseDate { get; } = DateTime.Now;

        [ForeignKey("User")]
		[Column("UserId")]
		public int UserId { get; private set; }

		private User _user;
		public User User
        {
            get => _user;
            set
            {
                if (value == null)
                    throw new ArgumentException("User is required.", nameof(User));

                _user = value;
                UserId = value.Id;
            }
        }

        [ForeignKey("Theme")]
		[Column("ThemeId")]
		public int ThemeId { get; private set; }

        private Theme _theme;
		public Theme Theme
        {
            get => _theme;
            set
            {
                if (value == null)
                    throw new ArgumentException("Theme is required.", nameof(Theme));

                _theme = value;
                ThemeId = value.Id;
            }
        }

        public override bool Equals(object? obj)
        {
            return obj is Voucher voucher &&
                string.Equals(voucher.Number, Number, StringComparison.OrdinalIgnoreCase);
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(StringComparer.OrdinalIgnoreCase.GetHashCode(Number ?? string.Empty));
        }

        public override string ToString()
        {
            return $"Voucher - Id: {Id}, Number: {Number}, Initial amount: {InitialAmount}";
        }
    }
}
