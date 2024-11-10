using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectManagementApi.Core.Models
{
    public class Project
    {
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        [DisplayName("Project")]
        public string Name { get; set; }

        [DisplayName("Version")]
        public string Version { get; set; }

        [DisplayName("Programming Language")]
        public string ProgrammingLanguage { get; set; }

        [DisplayName("Last Certification")]
        public DateTime? LastCertification { get; set; }

        public List<DevelopmentStage> Stages { get; set; }
        public List<Employee> Employees { get; set; }

    }
    public static class DbContextProject
    {
        public static void SetarDbContextProject(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>(
            mb =>
            {
                mb.HasMany(x => x.Stages)
                    .WithOne()
                    .HasForeignKey(x => x.ProjectId)
                    .OnDelete(DeleteBehavior.Cascade);

                mb.HasMany(x => x.Employees)
                    .WithOne()
                    .HasForeignKey(x => x.ProjectId)
                    .OnDelete(DeleteBehavior.SetNull);
            });
        }
    }
}
