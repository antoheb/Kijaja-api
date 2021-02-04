using System;

namespace Domain
{
    public class Address
    {
        public Guid Id {get; set;}

        public virtual AppUser AppUser { get; set; }

        public string AppUserId { get; set; }

        public string Country {get; set;}
        
        public string Province { get; set; }

        public string Street { get; set; }

        public string City { get; set; }

        public string PostalCode { get; set; }
    }
}