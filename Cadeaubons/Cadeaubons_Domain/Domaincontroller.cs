using Cadeaubons_Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadeaubons_Domain
{
    public class Domaincontroller
    {

        public User? login(string email, string password)
        {
            // tijdelijke testimplementatie
            if (email == "admin@test.be" && password == "Test123!")
            {
                return new User
                {
                    FirstName = "Admin",
                    LastName = "User",
                    Email = email
                };
            }

            return null;
        }

    }
}
