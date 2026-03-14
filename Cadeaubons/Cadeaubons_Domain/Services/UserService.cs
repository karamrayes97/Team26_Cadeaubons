using Cadeaubons_Domain.Model;
using Cadeaubons_Domain.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadeaubons_Domain.Services
{
	public class UserService
	{

		private Repository _repository;

		public UserService(Repository repository)
		{
			_repository = repository;
		}

		public List<User> GetUsers()
		{
			return _repository.Users.ToList();
		}

		public void AddUser(User user)
		{
			 _repository.Users.Add(user);
		}
	}
}
