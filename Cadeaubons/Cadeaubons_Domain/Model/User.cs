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
	[Table("Users")]
	public class User
	{
        private const int MINIMUM_AGE = 18;
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

		private string _firstName = string.Empty;
        [Column("FirstName")]
		public string FirstName
        {
            get => _firstName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("First name is required.", nameof(FirstName));

                _firstName = value.Trim();
            }
        }

		private string _lastName = string.Empty;
        [Column("LastName")]
		public string LastName
        {
            get => _lastName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Last name is required.", nameof(LastName));

                _lastName = value.Trim();
            }
        }

        private DateTime _dateOfBirth;
        [Column("DateOfBirth")]
		public DateTime DateOfBirth
        {
            get => _dateOfBirth;
            set {
                int age = DateTime.Today.Year - value.Year;

                if (value > DateTime.Today.AddYears(-age))
                    age--;

                if (age < MINIMUM_AGE)
                    throw new ArgumentException($"The minimum allowed age is {MINIMUM_AGE}.", nameof(DateOfBirth));

                _dateOfBirth = value;
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

        private string _email = string.Empty;
        [Column("Email")]
		public string Email 
        { 
            get => _email; 
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Email is required.", nameof(Email));

                value = value.Trim().ToLower();
                string pattern = @"^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,}$";

                if (!Regex.IsMatch(value, pattern))
                    throw new ArgumentException("Invalid email format.", nameof(Email));

                _email = value;
            }
        }

        private string _passwordSalt = string.Empty;
        [Column("PasswordSalt")]
        public string PasswordSalt
        {
            get => _passwordSalt;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("PasswordSalt is required.", nameof(PasswordSalt));

                if (!IsValidBase64(value))
                    throw new ArgumentException("PasswordSalt must be a valid Base64 string.", nameof(PasswordSalt));

                _passwordSalt = value;
            }
        }

        private string _passwordHash = string.Empty;
        [Column("PasswordHash")]
        public string PasswordHash
        {
            get => _passwordHash;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("PasswordHash is required.", nameof(PasswordHash));

                if (!IsValidBase64(value))
                    throw new ArgumentException("PasswordHash must be a valid Base64 string.", nameof(PasswordHash));

                _passwordHash = value;
            }
        }

        [Column("CreatedAt")]
		public DateTime CreatedAt { get;} = DateTime.Now;

		[Column("Role")]
		public Role Role { get; set; }

        [Column("IsActive")]
        public bool IsActive { get; set; } = true;

        public override bool Equals(object? obj)
        {
            return obj is User user &&
                string.Equals(user.Email, Email, StringComparison.OrdinalIgnoreCase);
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(StringComparer.OrdinalIgnoreCase.GetHashCode(Email ?? string.Empty));
        }

        public override string ToString()
        {
            return $"User - Id: {Id}, First name: {FirstName}, Last name: {LastName}, Email: {Email}";
        }

        private static bool IsValidBase64(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            try
            {
                Convert.FromBase64String(value);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
