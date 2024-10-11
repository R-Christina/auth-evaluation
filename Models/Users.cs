using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace auth.Models
{
    public class Users
    {
        [Key]
        public int user_id { get; set; }

        [ForeignKey("emp")]
        public int emp_id { get; set; }
        public int matricule { get; set; }
        public string email { get;set; }
        public string password { get; set; }

        [ForeignKey("role")]
        public int role_id { get; set ;}

        public string? resetToken { get; set; }
        public DateTime? resetTokenExpiration { get; set; }

        public Emp? emp { get; set; }
        public Role? role { get; set; }
    }
}