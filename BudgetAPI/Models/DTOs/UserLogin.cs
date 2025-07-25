using System.ComponentModel.DataAnnotations;

namespace BudgetAPI.Models.DTOs
{
    /// <summary>
    /// Représente les identifiants d’un utilisateur pour l’authentification (login).
    /// </summary>
    public class UserLogin
    {
        /// <summary>
        /// Nom d'utilisateur de connexion.
        /// </summary>
        /// <example>johndoe</example>
        [Required(ErrorMessage = "Le nom d'utilisateur est obligatoire.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Le nom d'utilisateur doit contenir entre 3 et 50 caractères.")]
        public string UserName { get; set; }

        /// <summary>
        /// Mot de passe associé à l'utilisateur.
        /// </summary>
        /// <example>MySecurePass123!</example>
        [Required(ErrorMessage = "Le mot de passe est obligatoire.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Le mot de passe doit contenir au moins 6 caractères.")]
        public string Password { get; set; }
    }
}
