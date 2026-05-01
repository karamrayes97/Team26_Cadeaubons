using Cadeaubons_Domain.DTO;
using Cadeaubons_Domain.Model;
using Cadeaubons_Domain.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadeaubons_Domain.Services
{
	public class PaymentService
	{
		private Repository _repository;

		public PaymentService(Repository repository)
		{
			_repository = repository;
		}

		public void AddPayment(PaymentDTO paymentDTO)
		{
			Payment payment = new Payment();
			payment.StripePaymentId = paymentDTO.StripeId;
			if (paymentDTO.PaymentSuccess)
			{
				payment.Status = PaymentStatus.Succeeded;
			}
			payment.VoucherId = paymentDTO.VoucherId;
			payment.Amount = paymentDTO.Amount;
			payment.Date = paymentDTO.Date;

			_repository.Add(payment);
			_repository.SaveChanges();
		}
	}
}
