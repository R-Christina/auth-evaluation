using BCrypt.Net;

namespace auth.Services
{
    public class PasswordHasher
    {
        // Méthode pour hasher un mot de passe
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        // Méthode pour vérifier si le mot de passe correspond au hash
        public bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
