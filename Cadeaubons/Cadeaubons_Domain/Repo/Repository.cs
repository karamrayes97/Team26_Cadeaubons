using Cadeaubons_Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadeaubons_Domain.Repo
{
	public class Repository : DbContext
	{
		public DbSet<User> Users { get; set; }

		public DbSet<Voucher> GiftCards { get; set; }

		public DbSet<Consumption> Consumptions { get; set; }

		public DbSet<Payment> Payments { get; set; }

		public DbSet<Theme> Themes { get; set; }

		public DbSet<Store> Stores { get; set; }

		public DbSet<City> Cities { get; set; }

		public Repository()
		{

		}

		public Repository(DbContextOptions<Repository> options) : base(options)
		{

		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured)
			{
				optionsBuilder.UseSqlServer(
					"Server=(localdb)\\MSSQLLocalDB;Database=GiftCardsDB;Trusted_Connection=True;TrustServerCertificate=True;");
			}
		}
	}
}
