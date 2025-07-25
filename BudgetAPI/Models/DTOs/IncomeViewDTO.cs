using System;
using System.ComponentModel.DataAnnotations;

namespace BudgetAPI.Models.DTOs
{
    /// <summary>
    /// Représente un revenu affiché dans les réponses de l'API.
    /// Contient les détails du revenu ainsi que le nom de la source.
    /// </summary>
    public class IncomeViewDTO
    {
        /// <summary>
        /// Identifiant unique du revenu.
        /// </summary>
        /// <example>1</example>
        public int Id { get; set; }

        /// <summary>
        /// Identifiant de la source associée.
        /// </summary>
        /// <example>2</example>
        [Required(ErrorMessage = "L'Id de la source est obligatoire.")]
        public int SourceId { get; set; }

        /// <summary>
        /// Nom de la source du revenu.
        /// </summary>
        /// <example>Salaire</example>
        [Required(ErrorMessage = "Le nom de la source est obligatoire.")]
        [StringLength(100, ErrorMessage = "La source ne doit pas dépasser 100 caractères.")]
        public string SourceName { get; set; }

        /// <summary>
        /// Montant du revenu.
        /// </summary>
        /// <example>2000.00</example>
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
