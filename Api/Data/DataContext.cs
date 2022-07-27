using Api.Api.Application;
using Microsoft.EntityFrameworkCore;

namespace Api.Data
{
    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions<DataContext> options) : base(options){}

        public DbSet<SuperHero> SuperHeroes { get; set; }

        public DbSet<User> Users { get; set; }
    }
}
