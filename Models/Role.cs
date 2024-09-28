using System.ComponentModel.DataAnnotations;

namespace auth.Models
{
    public class Role
    {
        [Key]
        public int role_id { get; set;}
        public string role_nom { get; set;}
        public virtual ICollection<Users> users { get; set; }
    }
}