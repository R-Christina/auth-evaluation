using System.ComponentModel.DataAnnotations;

namespace auth.Models
{
    public class ResetPassword
    {
        public string email { get;set; }
        public string Newpassword { get; set; }
        public string token { get; set; }
    }
}