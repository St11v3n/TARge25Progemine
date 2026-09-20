


using Microsoft.EntityFrameworkCore;
using TARge25Shop.Core.Domain;

namespace TARge25Shop.Data
{
    //nimetasime classi TARge25ShopContext, mis pärib DbContext klassi
    public class KindergartenContext : DbContext
    {
        //see class tuleb teha DbContextiks,
        //et saaks kasutada Entity Frameworki andmebaasi operatsioone
        //tegime konteksti, mis pärib DbContext klassi
        public KindergartenContext(DbContextOptions<KindergartenContext> options)
            : base(options) { }

        //vaja lisada dbSet, mis on seotud meie domain klassiga Spaceship
        public DbSet<Kindergarten> Kindergartens { get; set; }
    }



}
