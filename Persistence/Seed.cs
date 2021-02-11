using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Identity;

namespace Persistence
{
    public class Seed
    {
        public async static Task SeedData(DataContext context, UserManager<AppUser> userManager)
        {
            if (userManager.Users.FirstOrDefault(x => x.FirstName == "Antoine") == null)
            {
                var user = new AppUser
                {
                    FirstName = "Antoine",
                    LastName = "Graton",
                    Email = "merozwilliam@gmail.com",
                    UserName = "merozwilliam@gmail.com",
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(user, "Pa$$w0rd");

                var address = new Address
                {
                    Province = "QC",
                    Country = "Canada",
                    Street = "123 A Street",
                    City = "Montreal",
                    PostalCode = "J7C7L1",
                    AppUserId = user.Id
                };

                context.Addresses.Add(address);
                context.SaveChanges();

                if (!context.Ads.Any())
                {
                    var ads = new List<Ad>()
                {
                    new Ad()
                    {
                        Name = "Item 1",
                        Description = "Description 1",
                        Price = 10,
                        Category = "Category 1",
                        Picture = null,
                        Status = "Status 1",
                        AppUser = null,
                        AppUserId = user.Id
                    },
                     new Ad()
                    {
                        Name = "Item 2",
                        Description = "Description 2",
                        Price = 10,
                        Category = "Category 2",
                        Picture = null,
                        Status = "Status 2",
                        AppUser = null,
                        AppUserId = user.Id
                    },
                     new Ad()
                    {
                        Name = "Item 3",
                        Description = "Description 3",
                        Price = 10,
                        Category = "Category 3",
                        Picture = null,
                        Status = "Status 3",
                        AppUser = null,
                        AppUserId = null
                    },
                     new Ad()
                    {
                        Name = "Item 4",
                        Description = "Description 4",
                        Price = 10,
                        Category = "Category 4",
                        Picture = null,
                        Status = "Status 4",
                        AppUser = null,
                        AppUserId = null
                    },
                     new Ad()
                    {
                        Name = "Item 5",
                        Description = "Description 5",
                        Price = 10,
                        Category = "Category 5",
                        Picture = null,
                        Status = "Status 5",
                        AppUser = null,
                        AppUserId = null
                    }
                };

                    context.Ads.AddRange(ads);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}