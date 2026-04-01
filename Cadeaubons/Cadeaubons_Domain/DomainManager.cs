using Cadeaubons_Domain.DTO;
using Cadeaubons_Domain.Model;
using Cadeaubons_Domain.Repo;
using Cadeaubons_Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Cadeaubons_Domain
{
    public class DomainManager
    {
        private readonly UserService _userService;

		private readonly CityService _cityService;

		private readonly StoreService _storeService;


		public DomainManager(UserService userService , CityService cityService,StoreService storeService)
        {
            _userService = userService;
            _cityService = cityService;
            _storeService = storeService;
        }

        public List<UserDTO> GetUsers()
        {
            return _userService.GetUsers();
        }

        public UserDTO? GetByEmail(string email)
        {
            return _userService.GetByEmail(email);
        }

        public UserDTO RegisterUser(RegisterUserRequest request)
        {
            return _userService.RegisterUser(request);
        }

        public UserDTO Login(string email, string password)
        {
            return _userService.Login(email, password);
        }

       public List<CityDTO> GetCities()
        {
            return _cityService.GetAll();
        }

        public void AddStore(StoreDTO city)
        {
            _storeService.AddStore(city);
        }

	}
}
