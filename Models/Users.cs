using System.ComponentModel.DataAnnotations;

namespace auth.Models
{
    public class Users
    {
        [Key]
        public int user_id { get; set; }
        public int emp_id { get; set; }
        public int matricule { get; set; }
        public string password { get; set; }
        
    }
}