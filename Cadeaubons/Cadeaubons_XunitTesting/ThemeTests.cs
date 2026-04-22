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

        [Fact]
        public void Add_ShouldSaveThemeToDatabase()
        {
            var db = GetDbContext();
            var service = new ThemeService(db);

            var result = service.Add("Christmas", "Xmas theme", "🎄", "#FF0000");

            Assert.Equal("Christmas", result.Name);
            Assert.Equal("🎄", result.IconPath);
            Assert.Equal("#FF0000", result.PrimaryColor);
            Assert.Equal(3, db.Themes.Count());
        }

        [Fact]
        public void Add_ShouldThrow_WhenNameAlreadyExists()
        {
            var db = GetDbContext();
            var service = new ThemeService(db);

            Assert.Throws<InvalidOperationException>(() =>
                service.Add("Nature", "Duplicate", "🌿", "#00FF00"));
        }

        [Fact]
        public void Add_ShouldThrow_WhenNameAlreadyExistsCaseInsensitive()
        {
            var db = GetDbContext();
            var service = new ThemeService(db);

            Assert.Throws<InvalidOperationException>(() =>
                service.Add("NATURE", "Duplicate", "🌿", "#00FF00"));
        }

        [Fact]
        public void Add_ShouldThrow_WhenNameInvalid()
        {
            var db = GetDbContext();
            var service = new ThemeService(db);

            Assert.Throws<ArgumentException>(() =>
                service.Add("", "Description", "🎄", "#FF0000"));
        }

        // ===== Update tests =====

        [Fact]
        public void Update_ShouldChangeThemeProperties()
        {
            var db = GetDbContext();
            var service = new ThemeService(db);

            var result = service.Update(1, "Winter", "New desc", "❄", "#0000FF");

            Assert.Equal("Winter", result.Name);
            Assert.Equal("New desc", result.Description);
            Assert.Equal("❄", result.IconPath);
            Assert.Equal("#0000FF", result.PrimaryColor);
        }

        [Fact]
        public void Update_ShouldThrow_WhenThemeNotFound()
        {
            var db = GetDbContext();
            var service = new ThemeService(db);

            Assert.Throws<InvalidOperationException>(() =>
                service.Update(999, "Name", "Desc", "🎄", "#FF0000"));
        }

        [Fact]
        public void Update_ShouldThrow_WhenAnotherThemeHasSameName()
        {
            var db = GetDbContext();
            var service = new ThemeService(db);

            // Try to update theme 2 (Technology) with the name of theme 1 (Nature)
            Assert.Throws<InvalidOperationException>(() =>
                service.Update(2, "Nature", "Desc", "🎁", "#0000FF"));
        }

        [Fact]
        public void Update_ShouldAllowSameName_WhenSameTheme()
        {
            var db = GetDbContext();
            var service = new ThemeService(db);

            // Update theme 1 with its own name but different description
            var result = service.Update(1, "Nature", "Updated desc", "🌲", "#00FF00");

            Assert.Equal("Nature", result.Name);
            Assert.Equal("Updated desc", result.Description);
        }

        // ===== Delete tests =====

        [Fact]
        public void Delete_ShouldRemoveThemeFromDatabase()
        {
            var db = GetDbContext();
            var service = new ThemeService(db);

            service.Delete(1);

            Assert.Single(db.Themes);
            Assert.DoesNotContain(db.Themes, t => t.Id == 1);
        }

        [Fact]
        public void Delete_ShouldThrow_WhenThemeNotFound()
        {
            var db = GetDbContext();
            var service = new ThemeService(db);

            Assert.Throws<InvalidOperationException>(() => service.Delete(999));
        }


    }
}
