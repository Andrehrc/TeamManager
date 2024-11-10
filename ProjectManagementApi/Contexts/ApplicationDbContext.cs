using Microsoft.EntityFrameworkCore;
using ProjectManagementApi.Core.Models;

namespace ProjectManagementApi.Contexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserRefreshToken> RefreshTokens { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<DevelopmentStage> DevelopmentStages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.SetarDbContextUser();
            modelBuilder.SetarDbContextEmployees();
            modelBuilder.SetarDbContextProject();
            modelBuilder.Seed();
        }
    }

    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User>().HasData(new User[]
            {
                new User {
                    Id = 1,
                    Name = "User1",
                    Email ="user1email@test.com",
                    Password = "C2DaT6BcRZ3t/+RkPJJZXg==",
                    ConfirmationToken = Guid.NewGuid(),
                    EmailConfirmed = false
                },
                new User
                {
                    Id = 2,
                    Name = "User2",
                    Email ="user2email@test.com",
                    Password = "C2DaT6BcRZ3t/+RkPJJZXg==",
                    ConfirmationToken = Guid.NewGuid(),
                    EmailConfirmed = false
                }
            });
        }
    }

}
