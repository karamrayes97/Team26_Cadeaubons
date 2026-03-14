using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadeaubons_Domain.Model
{
	[Table("Consumptions")]
	public class Consumption
	{
		[Key]
		[Column("Id")]
		public int Id { get; set; }

		[Column("Amount", TypeName = "decimal(10,2)")]
		public decimal Amount { get; set; }

		[Column("Reason")]
		public string Reason { get; set; }

		[Column("Date")]
		public DateTime Date { get; set; }

		[ForeignKey("Voucher")]
		[Column("VoucherId")]
		public int VoucherId { get; set; }

		public Voucher Voucher { get; set; }

		[ForeignKey("Store")]
		[Column("StoreId")]
		public int StoreId { get; set; }

		public Store Store { get; set; }
	}
}
