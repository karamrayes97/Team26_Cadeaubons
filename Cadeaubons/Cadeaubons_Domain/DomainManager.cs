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
        private readonly ThemeService _themeService;

        private readonly VoucherService _voucherService;

		public DomainManager(UserService userService , CityService cityService,StoreService storeService, ThemeService themeService ,VoucherService voucherService)
        {
            _userService = userService;
            _cityService = cityService;
            _storeService = storeService;
            _themeService = themeService;
            _voucherService = voucherService;
        }

        public List<UserDTO> GetUsers()
        {
            return _userService.GetAll();
        }

        public UserDTO? GetByEmail(string email)
        {
            return _userService.GetByEmail(email);
        }

        public UserDTO RegisterUser(RegisterUserRequest request)
        {
            return _userService.Register(request);
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

        public List<ThemeDTO> GetAllThemes()
        {
            return _themeService.GetAll();
        }

        public void AddVoucher(VoucherDTO voucher)
        {
            _voucherService.AddVoucher(voucher);
        }

        public ThemeDTO AddTheme(string name, string description, string iconPath, string primaryColor)
        {
            return _themeService.Add(name, description, iconPath, primaryColor);
        }

        public ThemeDTO UpdateTheme(int id, string name, string description, string iconPath, string primaryColor)
        {
            return _themeService.Update(id, name, description, iconPath, primaryColor);
        }

        public void DeleteTheme(int id)
        {
            _themeService.Delete(id);
        }

    }
}
