using System.Collections.Generic;
using System.Linq;
using Domain;

namespace Persistence
{
    public class Seed
    {
        public static void SeedData(DataContext context)
        {
            if (!context.Items.Any())
            {
                var items = new List<Item>()
                {
                    new Item()
                    {
                        Name = "Item 1",
                        Description = "Description 1",
                        Price = 10,
                        Category = "Category 1",
                        Picture = null,
                        Status = "Status 1",
                        AppUser = null,
                        AppUserId = null
                    },
                     new Item()
                    {
                        Name = "Item 2",
                        Description = "Description 2",
                        Price = 10,
                        Category = "Category 2",
                        Picture = null,
                        Status = "Status 2",
                        AppUser = null,
                        AppUserId = null
                    },
                     new Item()
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
                     new Item()
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
                     new Item()
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

                context.Items.AddRange(items);
                context.SaveChanges();
            }
        }
    }
}