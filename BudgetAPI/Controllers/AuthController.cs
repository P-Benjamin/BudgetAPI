using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BudgetAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthController : ControllerBase
    {
        /// <summary>
        /// Récupère le nom d'utilisateur actuellement authentifié à partir du token JWT.
        /// </summary>
        /// <returns>Nom d'utilisateur contenu dans le token</returns>
        /// <response code="200">Nom d'utilisateur retourné avec succès</response>
        /// <response code="401">Utilisateur non authentifié</response>
        [HttpGet]
        public IActionResult Index()
        {
            var username = GetCurrentUserName();
            return Ok(username);
        }

        /// <summary>
        /// Extrait le nom d'utilisateur à partir des claims du token JWT.
        /// </summary>
        /// <returns>Nom d'utilisateur ou null si non trouvé</returns>
        private string GetCurrentUserName()
        {
            var identity = HttpContext.User.Claims;

            if (identity != null)
            {
                var claims = identity.ToList();
                var userName = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                return userName;
            }

            return null;
        }
    }
}
