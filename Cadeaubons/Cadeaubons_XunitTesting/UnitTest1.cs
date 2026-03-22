using Cadeaubons_Domain.Model;
using Cadeaubons_Domain.Repo;
using Microsoft.EntityFrameworkCore;
using System;
using Xunit;

namespace Cadeaubons_XunitTesting
{
	public class UnitTest1
	{
		private Repository GetDbContext()
		{
			var options = new DbContextOptionsBuilder<Repository>()
				.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
				.Options;

			return new Repository(options);
		}

		[Fact]
		public void Add_User_Should_Save_To_Database()
		{
			// Arrange
			var context = GetDbContext();

			var user = new User();
			user.FirstName = "karam";
			user.LastName = "rayes";
			user.Email = "karamrayes@gmail.com";
			user.PhoneNumber = "99999";
			user.DateOfBirth = new DateTime(1997,1,1);
			user.CreatedAt = DateTime.Now;
			user.IsActive = true;
			user.PasswordHash = "hash123";
			user.PasswordSalt = "salt123";
			user.Role = Role.Admin;

			// Act
			context.Users.Add(user);
			context.SaveChanges();

			// Assert
			var users = context.Users.ToList();

			Assert.Single(users);
			Assert.Equal("karam", users.First().FirstName);
		}
	}
}