using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Identity;

namespace Domain
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Province { get; set; }

        public string Street { get; set; }

        public string City { get; set; }

        public string PostalCode { get; set; }

        public Collection<Item> UserItems {get; set;}
    }
}