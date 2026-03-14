using Cadeaubons_Domain.Model;
using Cadeaubons_Domain.Repo;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Cadeaubons_Presistence
{
	public class Repository : DbContext, IRepository
	{
		public DbSet<User> Users { get; set; }

		public DbSet<Voucher> GiftCards { get; set; }

		public DbSet<Consumption> Consumptions { get; set; }

		public DbSet<Payment> Payments { get; set; }

		public DbSet<Theme> Themes { get; set; }

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
