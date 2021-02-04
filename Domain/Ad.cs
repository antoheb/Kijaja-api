using System;

namespace Domain
{
    public class Ad
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }

        public string Category { get; set; }

        public string Picture { get; set; }

        public string Status { get; set; }

        public virtual AppUser AppUser { get; set; }

        public string AppUserId { get; set; }

    }
}