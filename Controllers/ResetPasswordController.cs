using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Xml.Serialization;
using auth.Models;
using auth.Data.DbContexts;
using auth.Services;
namespace auth.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResetPasswordController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly EmailService _emailService;
        private readonly PasswordHasher _passwordHasher; 

        public ResetPasswordController(AppDbContext dbContext, EmailService emailService, PasswordHasher passwordHasher)
        {
            _dbContext = dbContext;
            _emailService = emailService;
            _passwordHasher = passwordHasher;
        }

        // Générer un token aléatoire pour l'utilisateur
        private string GenerateResetToken()
        {
            return Guid.NewGuid().ToString();
        }

        private async Task SendResetEmail(string email, string resetToken)
        {
            // Créer le lien de réinitialisation
            var resetLink = $"https://localhost:5163/reset-password?token={resetToken}";

            var message = $"Cliquez sur ce lien pour réinitialiser votre mot de passe : {resetLink}";

            // Utiliser le service d'email pour envoyer le message
            await _emailService.SendEmailAsync(email, "Réinitialisation de mot de passe", message);
        }

        [HttpPost("request-reset")]
        public async Task<IActionResult> RequestPasswordReset([FromBody] string email)
        {
            var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.email == email);
            if (user == null)
            {
                return NotFound("Utilisateur non trouvé");
            }

            var resetToken = GenerateResetToken();

            // Stocker le token et l'expiration dans la base de données
            user.resetToken = resetToken;
            user.resetTokenExpiration = DateTime.Now.AddHours(1); // Expiration dans 1 heure
            await _dbContext.SaveChangesAsync();

            try
            {
                await SendResetEmail(email, resetToken);
            }
            catch (Exception ex) // Vous pouvez spécifier un type d'exception plus précis, comme SmtpException
            {
                // Gérer l'erreur lors de l'envoi de l'e-mail
                return StatusCode(500, $"Erreur lors de l'envoi de l'e-mail : {ex.Message}");
            }

            return Ok("Email de réinitialisation envoyé");
        }


        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPassword request)
        {
            // On cherche l'utilisateur par son email
            var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.email == request.email);

            if (user == null)
            {
                return NotFound("Utilisateur introuvable.");
            }

            // Vérifiez si le token correspond et qu'il n'a pas expiré
            if (user.resetToken != request.token || user.resetTokenExpiration < DateTime.Now)
            {
                return BadRequest("Le lien de réinitialisation est invalide ou a expiré.");
            }

            // Mettre à jour le mot de passe (vous devriez hasher le mot de passe ici)
            user.password = _passwordHasher.HashPassword(request.Newpassword);

            // Effacer le token après la réinitialisation
            user.resetToken = null;
            user.resetTokenExpiration = null;

            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();

            return Ok("Mot de passe réinitialisé.");
        }

        [HttpPost("send-test-email")]
        public async Task<IActionResult> SendTestEmail()
        {
            try
            {
                await _emailService.SendEmailAsync("john@ravinala.aero", "Test Subject", "This is a test email body.");
                return Ok("Test email sent successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error sending email: {ex.Message}");
            }
        }
    } 
}