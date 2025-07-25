using System.ComponentModel.DataAnnotations;

namespace BudgetAPI.Models
{
    /// <summary>
    /// Représente un utilisateur de l'application.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Identifiant unique de l'utilisateur.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nom d'utilisateur (identifiant de connexion).
        /// </summary>
        [Required(ErrorMessage = "Le nom d'utilisateur est obligatoire.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Le nom d'utilisateur doit contenir entre 3 et 50 caractères.")]
        public string Username { get; set; }

        /// <summary>
        /// Mot de passe de l'utilisateur.
        /// </summary>
        [Required(ErrorMessage = "Le mot de passe est obligatoire.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Le mot de passe doit contenir au moins 6 caractères.")]
        public string Password { get; set; }

        /// <summary>
        /// Adresse email de l'utilisateur.
        /// </summary>
        [Required(ErrorMessage = "L'adresse email est obligatoire.")]
        [EmailAddress(ErrorMessage = "Adresse email invalide.")]
        public string EmailAddress { get; set; }

        /// <summary>
        /// Rôle de l'utilisateur (ex: Admin, Utilisateur).
        /// </summary>
        [Required(ErrorMessage = "Le rôle est obligatoire.")]
        [StringLength(20, ErrorMessage = "Le rôle ne doit pas dépasser 20 caractères.")]
        public string Role { get; set; }

        /// <summary>
        /// Nom de famille de l'utilisateur.
        /// </summary>
        [Required(ErrorMessage = "Le nom est obligatoire.")]
        [StringLength(50, ErrorMessage = "Le nom ne doit pas dépasser 50 caractères.")]
        public string Surname { get; set; }

        /// <summary>
        /// Prénom de l'utilisateur.
        /// </summary>
        [Required(ErrorMessage = "Le prénom est obligatoire.")]
        [StringLength(50, ErrorMessage = "Le prénom ne doit pas dépasser 50 caractères.")]
        public string GivenName { get; set; }
    }
}
