using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadeaubons_Domain.Model
{
	[Table("Users")]
	public class User
	{
		[Key]
		[Column("Id")]
		public int Id { get; set; }

		[Column("FirstName")]
		public string FirstName { get; set; }

		[Column("LastName")]
		public string LastName { get; set; }

		[Column("DateOfBirth")]
		public DateTime DateOfBirth { get; set; }

		[Column("PhoneNumber")]
		public string PhoneNumber { get; set; }

		[Column("Email")]
		public string Email { get; set; }

		[Column("PasswordSalt")]
		public string PasswordSalt { get; set; }

		[Column("PasswordHash")]
		public string PasswordHash { get; set; }

		[Column("CreatedAt")]
		public DateTime CreatedAt { get; set; }

		[Column("Role")]
		public Role Role { get; set; }

		[Column("IsActive")]
		public bool IsActive { get; set; }
	}
}
