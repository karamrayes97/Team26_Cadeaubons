using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cadeaubons_Domain.Model;

namespace Cadeaubons_Domain.DTO
{
    public sealed record UserDTO(
        int Id,
        string FirstName,
        string LastName,
        string PhoneNumber,
        string Email,
        DateTime DateOfBirth,
        DateTime CreatedAt,
        Role Role,
        bool IsActive
    )
    {
        public UserDTO(User user) : this(
            user.Id, user.FirstName, user.LastName, user.PhoneNumber,
            user.Email, user.DateOfBirth, user.CreatedAt, user.Role, user.IsActive
        )
        { }
    }
}
