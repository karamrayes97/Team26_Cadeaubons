using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadeaubons_Domain.Model
{
	[Table("Themes")]
	public class Theme
	{
		[Key]
		[Column("Id")]
		public int Id { get; set; }

		[Column("Name")]
		public string Name { get; set; }

		[Column("Description")]
		public string Description { get; set; }

		[Column("IconPath")]
		public string IconPath { get; set; }

		[Column("PrimaryColor")]
		public string PrimaryColor { get; set; }
	}
}
