using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadeaubons_Domain.Model
{
	[Table("Stores")]
	public class Store
	{
		[Key]
		[Column("Id")]
		public int Id { get; set; }

		[Column("Name")]
		public string Name { get; set; }

		[Column("Address")]
		public string Address { get; set; }

		[Column("PhoneNumber")]
		public string PhoneNumber { get; set; }

		[ForeignKey("City")]
		[Column("CityId")]
		public int CityId { get; set; }

		public City City { get; set; }
	}
}
