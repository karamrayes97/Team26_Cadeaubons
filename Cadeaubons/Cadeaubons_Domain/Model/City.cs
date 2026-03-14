using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadeaubons_Domain.Model
{
	[Table("Cities")]
	public class City
	{
		[Key]
		[Column("Id")]
		public int Id { get; set; }

		[Column("PostalCode")]
		public string PostalCode { get; set; }

		[Column("Name")]
		public string Name { get; set; }
	}
}
