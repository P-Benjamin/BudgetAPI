using BudgetAPI.Models;
using BudgetAPI.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BudgetAPI.Controllers
{
    /// <summary>
    /// Contrôleur responsable de l'authentification des utilisateurs et de la génération des jetons JWT.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly AccountContext _context;

        /// <summary>
        /// Constructeur du contrôleur de connexion.
        /// </summary>
        /// <param name="configuration">Configuration de l'application (accès aux clés JWT)</param>
        /// <param name="context">Contexte de base de données</param>
        public LoginController(IConfiguration configuration, AccountContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        /// <summary>
        /// Authentifie un utilisateur et génère un jeton JWT.
        /// </summary>
        /// <param name="userLogin">Nom d'utilisateur et mot de passe</param>
        /// <returns>Jeton JWT si les identifiants sont valides, sinon 401</returns>
        /// <response code="200">Connexion réussie, jeton JWT retourné</response>
        /// <response code="401">Identifiants invalides</response>
        [HttpPost]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult Login([FromBody] UserLogin userLogin)
        {
            var isOk = Authenticate(userLogin);

            if (isOk)
            {
                string token = Generate(userLogin);
                return Ok(token);
            }

            return Unauthorized("Bad credentials");
        }

        /// <summary>
        /// Génère un jeton JWT pour un utilisateur donné.
        /// </summary>
        /// <param name="userLogin">Données d'identification utilisateur</param>
        /// <returns>Chaîne du token JWT</returns>
        private string Generate(UserLogin userLogin)
        {
            var secret = _configuration["Jwt:Key"];

            var security = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var credentials = new SigningCredentials(security, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userLogin.UserName),
                new Claim("Coucou", "value") // Peut être modifié selon le besoin (rôle, ID, etc.)
            };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Vérifie si les identifiants utilisateur sont valides.
        /// </summary>
        /// <param name="userLogin">Identifiants fournis par le client</param>
        /// <returns><c>true</c> si l'utilisateur existe avec les bons identifiants, sinon <c>false</c></returns>
        private bool Authenticate(UserLogin userLogin)
        {
            var user = _context.User.FirstOrDefault(u =>
                u.Username.ToLower() == userLogin.UserName.ToLower()
                && u.Password == userLogin.Password); 

            return user != null;
        }
    }
}
