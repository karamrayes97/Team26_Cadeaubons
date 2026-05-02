using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cadeaubons_Domain.DTO;
using Cadeaubons_Domain.Model;
using Cadeaubons_Domain.Repo;

namespace Cadeaubons_Domain.Services
{
    public class ConsumptionService
    {
        private Repository _repository;

        public ConsumptionService(Repository repository)
        {
            _repository = repository;
        }

        public ConsumptionDTO Add(ConsumptionDTO consumptionDTO, decimal voucherRemainingAmount, bool IsVoucherExpired) {
            
            if (IsVoucherExpired)
                throw new InvalidOperationException("The voucher has expired and can no longer be used.");

            
            if (consumptionDTO.Amount > voucherRemainingAmount)
                throw new InvalidOperationException($"Insufficient voucher balance.");

            Consumption consumption = new Consumption()
            {
                Amount = consumptionDTO.Amount,
                Reason = consumptionDTO.Reason,
                VoucherId = consumptionDTO.VoucherId,
                StoreId = consumptionDTO.StoreId,
            };

            _repository.Add(consumption);
            _repository.SaveChanges();

            return new ConsumptionDTO(consumption);
        }
    }
}
