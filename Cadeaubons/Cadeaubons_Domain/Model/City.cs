using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Cadeaubons_Domain.Model
{
	[Table("Cities")]
	public class City
	{
        private const int POSTAL_CODE_LENGTH = 4;

        private int _id;
		[Key]
		[Column("Id")]
		public int Id 
        { 
            get => _id;
            set {
                if (value < 0)
                    throw new ArgumentException("Id must be zero or positive.", nameof(Id));

                _id = value;
            } 
        }

        private string _postalCode = string.Empty;
        [Column("PostalCode"), MaxLength(20)]
        public string PostalCode
        {
            get => _postalCode;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Postal code is required.", nameof(PostalCode));

                value = value.Trim();

                if (!Regex.IsMatch(value, @"^\d{4}$"))
                    throw new ArgumentException($"Postal code must be {POSTAL_CODE_LENGTH} digits.", nameof(PostalCode));

                _postalCode = value;
            }
        }

        private string _name = string.Empty;
        [Column("Name")]
        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Name is required.", nameof(Name));

                _name = value.Trim();
            }
        }

        public override bool Equals(object? obj)
        {
            return obj is City city &&
                string.Equals(city.PostalCode, PostalCode, StringComparison.OrdinalIgnoreCase);
        }
        public override int GetHashCode()
        {
            return HashCode.Combine( StringComparer.OrdinalIgnoreCase.GetHashCode(PostalCode ?? string.Empty));
        }

        public override string ToString() {
            return $"City - Id: {Id}, Name: {Name}, Postal code: {PostalCode}";
        }
    }
}
