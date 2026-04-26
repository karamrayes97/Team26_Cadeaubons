using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadeaubons_Domain.DTO
{
	public class VoucherDTO
	{
		public int InitialAmount { get; set; }

		public DateTime PurchaseDate { get; set; }

		public int BuyerId { get; set; }

		public int UserId { get; set; }

		public int ThemeId { get; set; }

		//public string Number { get; set; }
	}
}
