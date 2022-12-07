using Microsoft.EntityFrameworkCore;
using PustokDb2022.Models;
using System.Collections.Generic;

namespace PustokDb2022.DAL
{
    public class PustokDbContext : DbContext
    {
        public PustokDbContext(DbContextOptions<PustokDbContext> options) : base(options)
        {

        }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<BookImage> BookImages { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<BtmPromotion> BtmPromotions { get; set; }
        public DbSet<TopPromotion> TopPromotions { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Feature> Features { get; set; }
    }
}
