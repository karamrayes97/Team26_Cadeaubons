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

		public DbSet<Voucher> Vouchers { get; set; }

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

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Voucher>()
				.HasOne(v => v.User)
				.WithMany()
				.HasForeignKey(v => v.UserId)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<User>()
			.HasIndex(u => u.Email)
			.IsUnique();

			modelBuilder.Entity<City>().HasIndex(c => c.PostalCode).IsUnique();

			modelBuilder.Entity<Theme>().HasIndex(t => t.Name).IsUnique();

			modelBuilder.Entity<Voucher>().HasIndex(v => v.Number).IsUnique();

			modelBuilder.Entity<Payment>().HasIndex(p => p.StripePaymentId).IsUnique();



			//modelBuilder.Entity<Payment>()
			//	.HasOne(p => p.User)
			//	.WithMany()
			//	.HasForeignKey(p => p.UserId)
			//	.OnDelete(DeleteBehavior.Restrict);

			//modelBuilder.Entity<Consumption>()
			//	.HasOne(c => c.User)
			//	.WithMany()
			//	.HasForeignKey(c => c.UserId)
			//	.OnDelete(DeleteBehavior.Restrict);
		}
	}
}
