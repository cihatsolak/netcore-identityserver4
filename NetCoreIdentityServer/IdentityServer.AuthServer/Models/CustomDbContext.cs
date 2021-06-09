using Microsoft.EntityFrameworkCore;

namespace IdentityServer.AuthServer.Models
{
    public class CustomDbContext : DbContext
    {
        public CustomDbContext(DbContextOptions<CustomDbContext> options) : base(options)
        {
        }

        public DbSet<CustomUser> CustomUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CustomUser>().HasData(new CustomUser[]
            {
                new CustomUser
                {
                    Id = 1,
                    Email= "cihatsolak@test.com",
                    Password = "password",
                    City = "İstanbul",
                    Username = "cihat.solak"
                },
                new CustomUser
                {
                    Id = 2,
                    Email= "mesutsolak@test.com",
                    Password = "password",
                    City = "Ankara",
                    Username = "mesut.solak"
                },
                new CustomUser
                {
                    Id = 3,
                    Email= "mehmetsolak@test.com",
                    Password = "password",
                    City = "İzmir",
                    Username = "mehmet.solak"
                }
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
