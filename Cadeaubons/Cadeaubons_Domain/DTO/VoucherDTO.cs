using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadeaubons_Domain.DTO
{
	public class VoucherDTO
	{
		public decimal InitialAmount { get; set; }

		public DateTime PurchaseDate { get; set; }

		public int BuyerId { get; set; }

        public string BuyerFullName { get; set; } = string.Empty;

        public int UserId { get; set; }
        public string UserEmail { get; set; } = string.Empty;
        public string UserFullName { get; set; } = string.Empty;

        public int ThemeId { get; set; }
        public string ThemeName { get; set; } = string.Empty;

        
    }
}
