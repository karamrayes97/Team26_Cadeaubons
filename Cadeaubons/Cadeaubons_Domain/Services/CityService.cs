using Cadeaubons_Domain.DTO;
using Cadeaubons_Domain.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadeaubons_Domain.Services
{
	public class CityService
	{
		private readonly Repository _repository;

		public CityService(Repository repository)
		{
			_repository = repository;
		}

		public List<CityDTO> GetAll()
		{
			return _repository.Cities.Select(c => new CityDTO(c)).ToList();
		}
	}
}
