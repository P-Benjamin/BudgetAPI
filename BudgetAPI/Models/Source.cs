using System.ComponentModel.DataAnnotations;

namespace BudgetAPI.Models
{
    /// <summary>
    /// Représente une source de revenu ou de dépense.
    /// </summary>
    public class Source
    {
        /// <summary>
        /// Identifiant unique de la source.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nom de la source (ex : Salaire, Loyer, Investissement...).
        /// </summary>
        [Required(ErrorMessage = "Le nom de la source est obligatoire.")]
        [StringLength(100, ErrorMessage = "Le nom de la source ne doit pas dépasser 100 caractères.")]
        public string Name { get; set; }
    }
}
