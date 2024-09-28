using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace auth.Controllers
{
    [Authorize] // Protège toutes les actions de ce contrôleur avec un token JWT
    [ApiController]
    [Route("api/[controller]")]
    public class ProtectedController : ControllerBase
    {
        // Cette action ne sera accessible qu'avec un token JWT valide
        [HttpGet("data")]
        public IActionResult GetProtectedData()
        {
            return Ok("Données protégées accessibles uniquement avec un token valide.");
        }
    }
}