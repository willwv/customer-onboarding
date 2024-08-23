using Domain.Entities;
using Infrastructure.Databases.SqlServer;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using static Domain.Utils.Useful;

namespace WebApi.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class SqlServerExtensions
    {
        private static byte[]? MockImage = null;

        public static byte[] GetMockImage() 
        {
            if (MockImage == null)
            {
                string basePath = Directory.GetCurrentDirectory();
                string imagePath = Path.Combine(basePath, "MockImages", "cat.jpeg");
                MockImage = File.ReadAllBytes(imagePath);

                return MockImage;
            }
            else
            {
                return MockImage;
            }
        }
        public static IApplicationBuilder ApplySqlServerMigrations(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices
                                        .GetRequiredService<IServiceScopeFactory>()
                                        .CreateScope();
            using var context = serviceScope.ServiceProvider.GetService<SqlServerContext>();
            context?.Database.Migrate();

            return app;
        }

        public static IApplicationBuilder SeedSqlServerDatabase(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices
                            .GetRequiredService<IServiceScopeFactory>()
                            .CreateScope();

            using (var context = serviceScope.ServiceProvider.GetService<SqlServerContext>())
            {
                //Avoid to apply seeds again
                if (context.Users.Any() || Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Development")
                {
                    return app;
                }

                // User 1
                var admin1Salt = BCrypt.Net.BCrypt.GenerateSalt();
                var adm1 = new User
                {
                    Id = Guid.NewGuid(),
                    Name = "Admin1",
                    Email = "admin1@gmail.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Str0ngAnds3f3pa55!" + admin1Salt),
                    Salt = admin1Salt,
                    Role = Claims.SYS_ADMIN,
                    Logo = GetMockImage()
                };

                var adm1Addr = new Address
                {
                    Id = Guid.NewGuid(),
                    UserId = adm1.Id,
                    Street = "Random street adm1",
                    Number = "42 - A",
                    Neighborhood = "Imaginary",
                    City = "Any",
                    PostalCode = "12345678",
                    State = "GM",
                    Complement = "Cave"
                };

                // User 2
                var admin2Salt = BCrypt.Net.BCrypt.GenerateSalt();
                var adm2 = new User
                {
                    Id = Guid.NewGuid(),
                    Name = "Admin2",
                    Email = "admin2@gmail.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Secur3Pa$$word2!" + admin2Salt),
                    Salt = admin2Salt,
                    Role = Claims.SYS_ADMIN,
                    Logo = GetMockImage()
                };

                var adm2Addr = new Address
                {
                    Id = Guid.NewGuid(),
                    UserId = adm2.Id,
                    Street = "Example street adm2",
                    Number = "33 - B",
                    Neighborhood = "Fictional",
                    City = "Somewhere",
                    PostalCode = "87654321",
                    State = "XY",
                    Complement = "Near the park"
                };

                // User 3
                var admin3Salt = BCrypt.Net.BCrypt.GenerateSalt();
                var adm3 = new User
                {
                    Id = Guid.NewGuid(),
                    Name = "Admin3",
                    Email = "admin3@gmail.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("My$trongP@ssw0rd!" + admin3Salt),
                    Salt = admin3Salt,
                    Role = Claims.SYS_ADMIN,
                    Logo = GetMockImage()
                };

                var adm3Addr = new Address
                {
                    Id = Guid.NewGuid(),
                    UserId = adm3.Id,
                    Street = "Fable street adm3",
                    Number = "75 - C",
                    Neighborhood = "Mythical",
                    City = "Elsewhere",
                    PostalCode = "23456789",
                    State = "AB",
                    Complement = "Penthouse"
                };

                // User 4
                var admin4Salt = BCrypt.Net.BCrypt.GenerateSalt();
                var adm4 = new User
                {
                    Id = Guid.NewGuid(),
                    Name = "Admin4",
                    Email = "admin4@gmail.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Adm1nS3cur3P@ss!" + admin4Salt),
                    Salt = admin4Salt,
                    Role = Claims.SYS_ADMIN,
                    Logo = GetMockImage()
                };

                var adm4Addr = new Address
                {
                    Id = Guid.NewGuid(),
                    UserId = adm4.Id,
                    Street = "Story street adm4",
                    Number = "88 - D",
                    Neighborhood = "Legendary",
                    City = "Anywhere",
                    PostalCode = "34567890",
                    State = "YZ",
                    Complement = "Basement"
                };

                // User 5
                var admin5Salt = BCrypt.Net.BCrypt.GenerateSalt();
                var adm5 = new User
                {
                    Id = Guid.NewGuid(),
                    Name = "Admin5",
                    Email = "admin5@gmail.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Sup3rSecr3tP@ss!" + admin5Salt),
                    Salt = admin5Salt,
                    Role = Claims.SYS_ADMIN,
                    Logo = GetMockImage()
                };

                var adm5Addr = new Address
                {
                    Id = Guid.NewGuid(),
                    UserId = adm5.Id,
                    Street = "Fiction street adm5",
                    Number = "99 - E",
                    Neighborhood = "Fabled",
                    City = "Everywhere",
                    PostalCode = "45678901",
                    State = "CD",
                    Complement = "Attic"
                };

                // User 6
                var user6Salt = BCrypt.Net.BCrypt.GenerateSalt();
                var user6 = new User
                {
                    Id = Guid.NewGuid(),
                    Name = "User6",
                    Email = "user6@gmail.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Us3rP@ssw0rd6!" + user6Salt),
                    Salt = user6Salt,
                    Role = Claims.SYS_USER,
                    Logo = GetMockImage()
                };

                var user6Addr = new Address
                {
                    Id = Guid.NewGuid(),
                    UserId = user6.Id,
                    Street = "Random street user6",
                    Number = "15 - F",
                    Neighborhood = "Residential",
                    City = "Exampleville",
                    PostalCode = "11223344",
                    State = "EF",
                    Complement = "Apartment 2A"
                };

                // User 7
                var user7Salt = BCrypt.Net.BCrypt.GenerateSalt();
                var user7 = new User
                {
                    Id = Guid.NewGuid(),
                    Name = "User7",
                    Email = "user7@gmail.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("7S3cureP@ssword!" + user7Salt),
                    Salt = user7Salt,
                    Role = Claims.SYS_USER,
                    Logo = GetMockImage()
                };

                var user7Addr = new Address
                {
                    Id = Guid.NewGuid(),
                    UserId = user7.Id,
                    Street = "Imaginary street user7",
                    Number = "7 - G",
                    Neighborhood = "Suburbia",
                    City = "Fantasia",
                    PostalCode = "55667788",
                    State = "GH",
                    Complement = "House"
                };

                // User 8
                var user8Salt = BCrypt.Net.BCrypt.GenerateSalt();
                var user8 = new User
                {
                    Id = Guid.NewGuid(),
                    Name = "User8",
                    Email = "user8@gmail.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("User8Str0ngP@ss!" + user8Salt),
                    Salt = user8Salt,
                    Role = Claims.SYS_USER,
                    Logo = GetMockImage()
                };

                var user8Addr = new Address
                {
                    Id = Guid.NewGuid(),
                    UserId = user8.Id,
                    Street = "Fantasy street user8",
                    Number = "21 - H",
                    Neighborhood = "Dreamland",
                    City = "Wanderlust",
                    PostalCode = "99887766",
                    State = "IJ",
                    Complement = "Cottage"
                };

                // User 9
                var user9Salt = BCrypt.Net.BCrypt.GenerateSalt();
                var user9 = new User
                {
                    Id = Guid.NewGuid(),
                    Name = "User9",
                    Email = "user9@gmail.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("9SecureP@ssword!" + user9Salt),
                    Salt = user9Salt,
                    Role = Claims.SYS_USER,
                    Logo = GetMockImage()
                };

                var user9Addr = new Address
                {
                    Id = Guid.NewGuid(),
                    UserId = user9.Id,
                    Street = "Legend street user9",
                    Number = "30 - I",
                    Neighborhood = "Heroic",
                    City = "Adventure",
                    PostalCode = "33445566",
                    State = "KL",
                    Complement = "Villa"
                };

                // User 10
                var user10Salt = BCrypt.Net.BCrypt.GenerateSalt();
                var user10 = new User
                {
                    Id = Guid.NewGuid(),
                    Name = "User10",
                    Email = "user10@gmail.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Us3rTenS3cure!" + user10Salt),
                    Salt = user10Salt,
                    Role = Claims.SYS_USER,
                    Logo = GetMockImage()
                };

                var user10Addr = new Address
                {
                    Id = Guid.NewGuid(),
                    UserId = user10.Id,
                    Street = "Myth street user10",
                    Number = "12 - J",
                    Neighborhood = "Epic",
                    City = "Legends",
                    PostalCode = "66778899",
                    State = "MN",
                    Complement = "Bungalow"
                };
               
                context.Users.AddRange(new List<User> { adm1, adm2, adm3, adm4, adm5, user6, user7, user8, user9, user10 });
                context.SaveChanges();
                
                context.Address.AddRange(new List<Address> { adm1Addr, adm2Addr, adm3Addr, adm4Addr, adm5Addr, user6Addr, user7Addr, user8Addr, user9Addr, user10Addr });
                context.SaveChanges();

            }

            return app;
        }
    }
}
