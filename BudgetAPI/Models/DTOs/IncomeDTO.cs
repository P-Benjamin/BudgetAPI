using System;
using System.ComponentModel.DataAnnotations;

namespace BudgetAPI.Models.DTOs
{
    /// <summary>
    /// Représente les données d'entrée nécessaires pour créer ou modifier un revenu.
    /// </summary>
    public class IncomeDTO
    {
        /// <summary>
        /// Identifiant du revenu (utilisé pour les mises à jour).
        /// </summary>
        /// <example>1</example>
        public int Id { get; set; }

        /// <summary>
        /// Identifiant de la source de revenu associée.
        /// </summary>
        /// <example>3</example>
        [Required(ErrorMessage = "L'Id de la source est obligatoire.")]
        public int SourceId { get; set; }

        /// <summary>
        /// Montant du revenu.
        /// </summary>
        /// <example>1200.50</example>
        [Required(ErrorMessage = "Le montant est obligatoire.")]
        [Range(0.01, 1_000_000, ErrorMessage = "Le montant doit être supérieur à 0.")]
        public decimal Amount { get; set; }

        /// <summary>
        /// Date de réception du revenu.
        /// </summary>
        /// <example>2025-07-24</example>
        [Required(ErrorMessage = "La date est obligatoire.")]
        [DataType(DataType.Date, ErrorMessage = "Date invalide.")]
        public DateTime DateReceived { get; set; }
    }
}
