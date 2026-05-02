using Cadeaubons_Domain.DTO;
using Cadeaubons_Domain.Model;
using Cadeaubons_Domain.Repo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadeaubons_Domain.Services
{
	public class StoreService
	{
		private readonly Repository _repository;

		public StoreService(Repository repository)
		{
			_repository = repository;
		}

		public void AddStore(StoreDTO storeDTO)
		{
			Store store = new Store();
			store.Name = storeDTO.Name;
			store.Address = storeDTO.Adress;
			store.PhoneNumber = storeDTO.PhoneNumber;
			store.CityId = storeDTO.City.Id;
			_repository.Add(store);
			_repository.SaveChanges();
		}

        public List<StoreDTO> GetAll()
		{
			return _repository
				.Stores
				.Include(s => s.City)
				.Select(s => new StoreDTO(s)).ToList();
        }
    }
}
