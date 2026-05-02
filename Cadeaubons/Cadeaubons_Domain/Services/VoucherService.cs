using Cadeaubons_Domain.DTO;
using Cadeaubons_Domain.Model;
using Cadeaubons_Domain.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace Cadeaubons_Domain.Services
{
	public class VoucherService
	{
		private readonly Repository _repository;

		public VoucherService(Repository repository)
		{
			_repository = repository;
		}

		public Voucher AddVoucher(VoucherDTO voucherDTO)
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
			return voucher;
		}
	

		public List<VoucherOverviewDTO> GetVouchersForUser(int userId)
		{
			var vouchers = _repository.Vouchers
				.Include(v => v.Theme)
				.Include(v => v.Buyer)
				.Include(v => v.User)
				.Where(v => v.BuyerId == userId || v.UserId == userId)
				.OrderByDescending(v => v.PurchaseDate)
				.ToList();

			var voucherIds = vouchers.Select(v => v.Id).ToList();

			var consumptions = _repository.Consumptions
				.Include(c => c.Store)
				.Where(c => voucherIds.Contains(c.VoucherId))
				.ToList();

			return vouchers
				.Select(v => new VoucherOverviewDTO(
					v,
					consumptions.Where(c => c.VoucherId == v.Id)))
				.ToList();
		}
    } 
}
