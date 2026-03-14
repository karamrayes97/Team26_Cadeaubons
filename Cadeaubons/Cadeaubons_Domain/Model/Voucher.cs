using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadeaubons_Domain.Model
{
	[Table("Vouchers")]
	public class Voucher
	{
		[Key]
		[Column("Id")]
		public int Id { get; set; }

		[Column("Number")]
		public string Number { get; set; }

		[Column("InitialAmount", TypeName = "decimal(10,2)")]
		public decimal InitialAmount { get; set; }

		[Column("PurchaseDate")]
		public DateTime PurchaseDate { get; set; }

		[ForeignKey("User")]
		[Column("UserId")]
		public int UserId { get; set; }

		public User User { get; set; }

		[ForeignKey("Theme")]
		[Column("ThemeId")]
		public int ThemeId { get; set; }

		public Theme Theme { get; set; }
	}
}
