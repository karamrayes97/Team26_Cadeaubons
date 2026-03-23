using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Cadeaubons_Domain.Model
{
	[Table("Stores")]
	public class Store
	{
        private int _id;
        [Key]
		[Column("Id")]
		public int Id
        {
            get => _id;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Id must be zero or positive.", nameof(Id));

                _id = value;
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

        private string _address = string.Empty;
        [Column("Address")]
		public string Address 
        { 
            get => _address;
            set {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Address is required.", nameof(Address));
                
                _address = value.Trim();
            }
        }

        private string _phoneNumber = string.Empty;
        [Column("PhoneNumber")]
        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    _phoneNumber = string.Empty;
                    return;
                }

                value = value.Trim();
                string pattern = @"^\+?(\d{1,4}|\(\d{1,4}\))([\s.-]?\d+)*$";

                if (!Regex.IsMatch(value, pattern))
                    throw new ArgumentException("Invalid phone number format.", nameof(PhoneNumber));

                _phoneNumber = value;
            }
        }

        [ForeignKey("City")]
		[Column("CityId")]
        public int CityId { get; private set; }

        private City _city;
		public City City 
        { 
            get => _city;
            set { 
                if (value == null)
                    throw new ArgumentException("City is required.", nameof(City));

                _city = value;
                CityId = value.Id;
            } 
        }

        public override bool Equals(object? obj)
        {
            return obj is Store store &&
                string.Equals(store.Address, Address, StringComparison.OrdinalIgnoreCase) &&
                Equals(store.City, City);
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(
                StringComparer.OrdinalIgnoreCase.GetHashCode(Address ?? string.Empty),
                City
            );
        }

        public override string ToString()
        {
            return $"Store - Id: {Id}, Name: {Name}, Address: {Address}\n{City?.PostalCode}, {City?.Name}";
        }
    }
}
