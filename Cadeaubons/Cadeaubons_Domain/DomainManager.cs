using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cadeaubons_Domain.DTO;
using Cadeaubons_Domain.Model;
using Cadeaubons_Domain.Repo;
using Cadeaubons_Domain.Services;

namespace Cadeaubons_Domain
{
    public class DomainManager
    {
        private readonly UserService _userService;

        public DomainManager(UserService userService)
        {
            _userService = userService;
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
    }
}
