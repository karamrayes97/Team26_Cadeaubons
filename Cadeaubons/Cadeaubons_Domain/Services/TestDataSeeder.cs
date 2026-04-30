using Cadeaubons_Domain.Model;
using Cadeaubons_Domain.Repo;
using Cadeaubons_Domain.Security;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cadeaubons_Domain.Services
{
    public static class TestDataSeeder
    {
        // Universeel paswoord voor alle test-users => "Test123!"
        private const string DEFAULT_PASSWORD = "Test123!";

        private static readonly Random _rnd = new Random(42); // vaste seed voor reproduceerbare data

        public static void Seed(Repository repo)
        {
            // Skip als er al data is — voorkomt dubbele seed bij elke opstart
            if (repo.Users.Any() || repo.Themes.Any() || repo.Vouchers.Any())
                return;

            SeedThemes(repo);
            SeedCitiesAndStores(repo);
            SeedUsers(repo);
            repo.SaveChanges();

            SeedVouchersAndConsumptions(repo);
            repo.SaveChanges();
        }

        // --------- 10 Themes ---------
        private static void SeedThemes(Repository repo)
        {
            var themes = new[]
            {
                ("Verjaardag",   "Voor een vrolijke verjaardag",        "🎂", "#FF6B6B"),
                ("Kerstmis",     "Onder de kerstboom",                  "🎄", "#2E7D32"),
                ("Valentijn",    "Voor je geliefde",                    "❤️", "#E91E63"),
                ("Bedankt",      "Een warm bedankje",                   "🙏", "#FFB300"),
                ("Huwelijk",     "Voor het bruidspaar",                 "💍", "#C0CA33"),
                ("Geboorte",     "Welkom kleine spruit",                "👶", "#81D4FA"),
                ("Communie",     "Een speciale dag",                    "✨", "#9575CD"),
                ("Vaderdag",     "Voor de beste papa",                  "👔", "#5D4037"),
                ("Moederdag",    "Voor de beste mama",                  "🌷", "#F06292"),
                ("Diploma",      "Proficiat met je diploma",            "🎓", "#1976D2"),
            };

            foreach (var (name, desc, icon, color) in themes)
            {
                repo.Themes.Add(new Theme
                {
                    Name = name,
                    Description = desc,
                    IconPath = icon,
                    PrimaryColor = color
                });
            }
        }

        // --------- Cities + Stores (nodig voor Consumptions) ---------
        private static void SeedCitiesAndStores(Repository repo)
        {
            var gent = new City { Name = "Gent", PostalCode = "9000" };
            var brus = new City { Name = "Brussel", PostalCode = "1000" };
            var antw = new City { Name = "Antwerpen", PostalCode = "2000" };
            repo.Cities.AddRange(gent, brus, antw);

            repo.Stores.AddRange(
                new Store { Name = "Boekenhuis Gent", Address = "Kouter 12", PhoneNumber = "092224455", City = gent },
                new Store { Name = "Cinema City", Address = "Nieuwstraat 5", PhoneNumber = "022001122", City = brus },
                new Store { Name = "Restaurant Lumi", Address = "Meir 88", PhoneNumber = "032330099", City = antw }
            );
        }

        // --------- 5 Users ---------
        private static void SeedUsers(Repository repo)
        {
            // 1 Admin + 5 Customers, zodat je meteen ook admin-functies kan testen
            repo.Users.Add(BuildUser("Admin", "Test", "admin@test.be", Role.Admin));

            repo.Users.Add(BuildUser("Anna", "Janssens", "anna@test.be", Role.Customer));
            repo.Users.Add(BuildUser("Bram", "Peeters", "bram@test.be", Role.Customer));
            repo.Users.Add(BuildUser("Cindy", "Dhondt", "cindy@test.be", Role.Customer));
            repo.Users.Add(BuildUser("David", "Maes", "david@test.be", Role.Customer));
            repo.Users.Add(BuildUser("Emma", "Vermeulen", "emma@test.be", Role.Customer));
        }

        private static User BuildUser(string firstName, string lastName, string email, Role role)
        {
            string salt = PasswordHelper.GenerateSalt();
            string hash = PasswordHelper.HashPassword(DEFAULT_PASSWORD, salt);

            return new User
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                PhoneNumber = "0470000000",
                DateOfBirth = new DateTime(1990, 1, 1),
                Role = role,
                IsActive = true,
                PasswordSalt = salt,
                PasswordHash = hash
            };
        }

        // --------- Vouchers (0 t/m 4 per user) + consumpties ---------
        private static void SeedVouchersAndConsumptions(Repository repo)
        {
            var customers = repo.Users.Where(u => u.Role == Role.Customer).ToList();
            var themes = repo.Themes.ToList();
            var stores = repo.Stores.ToList();

            // Per customer een wisselend aantal aangekochte bons (0..4)
            int[] aantalPerUser = { 0, 1, 2, 3, 4 };

            for (int i = 0; i < customers.Count; i++)
            {
                var buyer = customers[i];
                int aantal = aantalPerUser[i];

                for (int v = 0; v < aantal; v++)
                {
                    // Ontvanger = random andere customer (50% kans op zelf, om alle gevallen te dekken)
                    var receiver = _rnd.Next(2) == 0
                        ? buyer
                        : customers[_rnd.Next(customers.Count)];

                    var theme = themes[_rnd.Next(themes.Count)];

                    // Aankoopdatum tussen 11 maanden geleden en vandaag — zodat sommige bons bijna verlopen zijn
                    int daysAgo = _rnd.Next(0, 330);
                    var purchaseDate = DateTime.Now.AddDays(-daysAgo);

                    decimal[] amounts = { 25m, 50m, 75m, 100m, 150m, 200m };
                    decimal initialAmount = amounts[_rnd.Next(amounts.Length)];

                    var voucher = new Voucher
                    {
                        Number = VoucherNumberHelper.GenerateNumber(),
                        InitialAmount = initialAmount,
                        PurchaseDate = purchaseDate,
                        BuyerId = buyer.Id,
                        UserId = receiver.Id,
                        ThemeId = theme.Id
                    };

                    repo.Vouchers.Add(voucher);
                    repo.SaveChanges(); // we hebben Id nodig voor Consumption

                    // 0 t/m 3 consumpties op deze bon, totaal mag InitialAmount niet overschrijden
                    int aantalConsumpties = _rnd.Next(0, 4);
                    decimal restant = initialAmount;

                    for (int c = 0; c < aantalConsumpties && restant > 5m; c++)
                    {
                        decimal amount = Math.Round(
                            (decimal)(_rnd.NextDouble() * (double)Math.Min(restant, 50m)) + 5m,
                            2);
                        if (amount > restant) amount = restant;

                        var store = stores[_rnd.Next(stores.Count)];
                        string[] reasons = { "Boek gekocht", "Avondje uit", "Bloemen", "Cadeau", "Etentje" };

                        repo.Consumptions.Add(new Consumption
                        {
                            Amount = amount,
                            Reason = reasons[_rnd.Next(reasons.Length)],
                            Voucher = voucher,
                            Store = store
                        });

                        restant -= amount;
                    }
                }
            }
        }
    }
}