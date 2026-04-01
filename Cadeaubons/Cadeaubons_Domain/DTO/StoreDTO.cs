using Cadeaubons_Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadeaubons_Domain.DTO
{
	public class StoreDTO
	{
		public string Name { get; set; }

		public string Adress { get; set; }

		public string PhoneNumber { get; set; }

		public CityDTO City { get; set; }

		public StoreDTO()
		{
			
		}

		public StoreDTO(Store store)
		{
			this.Name = store.Name;
			this.Adress = store.Address;
			this.PhoneNumber = store.PhoneNumber;
			this.City.Name = store.City.Name;
			this.City.ZipCode = store.City.PostalCode;
		}
	}
}
