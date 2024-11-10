using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectManagementApi.Core.Models
{
    public class DevelopmentStage
    {
        public int Id { get; set; }

        [ForeignKey("Project")]
        public int ProjectId { get; set; }

        [DisplayName("Development Stage")]
        public string DevelopmentStageName { get; set; }

        [DisplayName("Development Percentage")]
        [Precision(10, 2)]
        public decimal DevelopmentPercentage { get; set; }

        [NotMapped]
        public bool Delete { get; set; } = false;
    }
}
