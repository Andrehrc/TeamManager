using ProjectManagementApi.Core.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectManagementApi.Core.Models
{
    public class UserRefreshToken
    {
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        public string RefreshToken { get; set; }

        public DateTime Expiration { get; set; }

        public virtual User User { get; set; }
    }
}
