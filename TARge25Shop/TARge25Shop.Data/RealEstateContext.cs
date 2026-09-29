using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TARge25Shop.Core.Domain;

namespace TARge25Shop.Data
{
    public class RealEstateContext : DbContext
    {
        public RealEstateContext (DbContextOptions<RealEstateContext>options)
            :base(options) { }
        public DbSet<RealEstate> RealEstates { get; set; }
    }
}
