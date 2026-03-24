using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cadeaubons_Domain.DTO;
using Cadeaubons_Domain.Model;
using Cadeaubons_Domain.Repo;
using Cadeaubons_Domain.Security;

namespace Cadeaubons_Domain.Services
{
	public class UserService
	{
        private HashSet<string> ADMINS = new (StringComparer.OrdinalIgnoreCase) {
            "hasan.jaban@student.hogent.be",
            "dagmar.mergaert@student.hogent.be",
            "karam.rayes@student.hogent.be"
        }; 

		private readonly Repository _repository;

		public UserService(Repository repository)
		{
			_repository = repository;
		}

		public List<UserDTO> GetUsers()
		{
			return _repository
                .Users
                .Select(u => new UserDTO(u))
                .ToList();
		}

        public UserDTO? GetByEmail(string email)
        {
            User? user = GetUserModelByEmail(email);
            return user == null ? null : new UserDTO(user);
        }

        public UserDTO RegisterUser(RegisterUserRequest request)
        {
            if (GetUserModelByEmail(request.Email) != null)
                throw new InvalidOperationException("A user with this email already exists.");

            User user = new User()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                DateOfBirth = request.DateOfBirth
            };
            user.PasswordSalt = PasswordHelper.GenerateSalt();
            user.PasswordHash = PasswordHelper.HashPassword(request.Password, user.PasswordSalt);
            user.Role = ADMINS.Contains(request.Email) ? Role.Admin : Role.Customer;

            _repository.Users.Add(user);
            _repository.SaveChanges();

            return new UserDTO(user);
        }

        public UserDTO Login(string email, string password)
        {
            User? user = GetUserModelByEmail(email);
            if (user == null || !PasswordHelper.VerifyPassword(password, user.PasswordSalt, user.PasswordHash))
                throw new InvalidOperationException("Invalid email or password.");

            if (!user.IsActive)
                throw new InvalidOperationException("This account is not active");

            return new UserDTO(user);
        }

        private User? GetUserModelByEmail(string email)
        {
            return _repository
                .Users
                .FirstOrDefault(u => string.Equals(email.Trim(), u.Email, StringComparison.OrdinalIgnoreCase));
        }
    }
}
