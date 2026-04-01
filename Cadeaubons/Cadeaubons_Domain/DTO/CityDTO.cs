using Cadeaubons_Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadeaubons_Domain.DTO
{
	public class CityDTO
	{
		public int Id { get; set; }
		public string Name { get; set; }

		public string ZipCode { get; set; }

		public CityDTO()
		{
			
		}

		public CityDTO(City city)
		{
			this.Id = city.Id;
			this.Name = city.Name;
			this.ZipCode = city.PostalCode;
		}

		public override string ToString()
		{
			return $"{this.Name} {this.ZipCode}";
		}
	}
}
