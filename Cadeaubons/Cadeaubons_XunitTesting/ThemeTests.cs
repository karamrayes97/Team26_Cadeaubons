using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cadeaubons_Domain.Model;
using Cadeaubons_Domain.Repo;
using Cadeaubons_Domain.Services;
using Microsoft.EntityFrameworkCore;

namespace Cadeaubons_XunitTesting
{
    public class ThemeTests
    {
        private Repository GetDbContext()
        {
            var options = new DbContextOptionsBuilder<Repository>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new Repository(options);
            context.Themes.AddRange(
                new Theme { Id = 1, Name = "Nature" },
                new Theme { Id = 2, Name = "Technology" }
            );
            context.SaveChanges();

            return context;
        }

        [Fact]
        public void GetAll_ShouldReturnAllThemes()
        {
            var db = GetDbContext();
            var service = new ThemeService(db);

            var result = service.GetAll();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, t => t.Name == "Nature");
            Assert.Contains(result, t => t.Name == "Technology");
        }

        [Theory]
        [InlineData("Nature", true)]
        [InlineData("technology", true)]
        [InlineData("Unknown", false)]
        public void GetByName_ShouldReturnCorrectTheme(string name, bool exists)
        {
            var db = GetDbContext();
            var service = new ThemeService(db);

            var result = service.GetByName(name);

            if (exists)
            {
                Assert.NotNull(result);
                Assert.Equal(name.Trim(), result.Name, ignoreCase: true);
            }
            else
            {
                Assert.Null(result);
            }
        }
    }
}
