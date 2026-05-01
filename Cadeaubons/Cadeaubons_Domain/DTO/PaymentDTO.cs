using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadeaubons_Domain.DTO
{
	public class PaymentDTO
	{
		public decimal Amount { get; set; }

		public bool PaymentSuccess { get; set; }

		public string StripeId { get; set; }

		public int VoucherId { get; set; }

		public DateTime Date { get; set; }
	}
}
