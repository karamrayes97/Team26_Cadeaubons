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
	public class VoucherService
	{
		private readonly Repository _repository;

		public VoucherService(Repository repository)
		{
			_repository = repository;
		}

		public void AddVoucher(VoucherDTO voucherDTO)
		{
			Voucher voucher = new Voucher
			{
				BuyerId = voucherDTO.BuyerId,
				UserId = voucherDTO.UserId,
				InitialAmount = voucherDTO.InitialAmount,
				ThemeId = voucherDTO.ThemeId,
				Number = VoucherNumberHelper.GenerateNumber(),
				PurchaseDate = voucherDTO.PurchaseDate,
			};
			_repository.Add(voucher);
			_repository.SaveChanges();
		}
	}
}
