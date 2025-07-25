using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BudgetAPI.Controllers
{
    /// <summary>
    /// Contrôleur d'authentification permettant de récupérer les informations de l'utilisateur connecté.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthController : ControllerBase
    {
        /// <summary>
        /// Récupère le nom d'utilisateur actuellement authentifié à partir du token JWT.
        /// </summary>
        /// <remarks>
        /// Ce point de terminaison lit les claims du token JWT pour extraire l'identifiant de l'utilisateur (claim de type <c>NameIdentifier</c>).
        /// </remarks>
        /// <returns>Le nom d'utilisateur extrait du token JWT.</returns>
        /// <response code="200">Nom d'utilisateur retourné avec succès.</response>
        /// <response code="401">Accès non autorisé. Le token JWT est manquant ou invalide.</response>
        [HttpGet]
        public IActionResult Index()
        {
            var username = GetCurrentUserName();
            return Ok(username);
        }

        /// <summary>
        /// Extrait le nom d'utilisateur à partir des claims du token JWT.
        /// </summary>
        /// <remarks>
        /// Cherche un claim de type <c>ClaimTypes.NameIdentifier</c> dans l'objet HttpContext.User.
        /// </remarks>
        /// <returns>Le nom d'utilisateur ou <c>null</c> si aucun claim correspondant n'est trouvé.</returns>
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
