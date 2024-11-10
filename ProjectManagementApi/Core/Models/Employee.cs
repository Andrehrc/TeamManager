using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectManagementApi.Core.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [ForeignKey("Project")]
        public int? ProjectId { get; set; }

        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        public string TechnicalExperienceLevel { get; set; }

        public string Name { get; set; }

        public string Position { get; set; }

        public string ImagePath { get; set; } = string.Empty;
    }

    public static class DbContextEmployees
    {
        public static void SetarDbContextEmployees(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>(mb =>
            {
                mb.HasOne<User>()
                  .WithMany()
                  .HasForeignKey(x => x.UserId) 
                  .OnDelete(DeleteBehavior.NoAction);
            });
        }
    }
}
