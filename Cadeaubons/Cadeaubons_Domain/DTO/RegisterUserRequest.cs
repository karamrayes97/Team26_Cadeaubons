using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadeaubons_Domain.DTO
{
    public sealed record RegisterUserRequest(
        string FirstName,
        string LastName,
        string PhoneNumber,
        string Email,
        DateTime DateOfBirth,
        string Password
        ) {}
}
