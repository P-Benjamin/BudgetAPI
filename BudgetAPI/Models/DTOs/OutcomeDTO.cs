using System;
using System.ComponentModel.DataAnnotations;

namespace BudgetAPI.Models.DTOs
{
    /// <summary>
    /// Représente les données nécessaires à la création ou à la modification d'une dépense.
    /// </summary>
    public class OutcomeDTO
    {
        /// <summary>
        /// Identifiant unique de la dépense (utilisé pour la mise à jour).
        /// </summary>
        /// <example>1</example>
        public int Id { get; set; }

        /// <summary>
        /// Identifiant de la source associée à la dépense.
        /// </summary>
        /// <example>3</example>
        [Required(ErrorMessage = "L'Id de la source est obligatoire.")]
        public int SourceId { get; set; }

        /// <summary>
        /// Montant de la dépense.
        /// </summary>
        /// <example>250.75</example>
        [Required(ErrorMessage = "Le montant est obligatoire.")]
        [Range(0.01, 1_000_000, ErrorMessage = "Le montant doit être supérieur à 0.")]
        public decimal Amount { get; set; }

        /// <summary>
        /// Date à laquelle la dépense a été effectuée.
        /// </summary>
        /// <example>2025-07-15</example>
        [Required(ErrorMessage = "La date est obligatoire.")]
        [DataType(DataType.Date, ErrorMessage = "Date invalide.")]
        public DateTime DateReceived { get; set; }
    }
}
