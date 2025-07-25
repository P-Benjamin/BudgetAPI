using System;
using System.ComponentModel.DataAnnotations;

namespace BudgetAPI.Models.DTOs
{
    /// <summary>
    /// Représente une dépense retournée par l'API.
    /// Contient les informations détaillées sur la source, le montant et la date.
    /// </summary>
    public class OutcomeViewDTO
    {
        /// <summary>
        /// Identifiant unique de la dépense.
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
        /// Nom de la source de la dépense.
        /// </summary>
        /// <example>Loyer</example>
        [Required(ErrorMessage = "Le nom de la source est obligatoire.")]
        [StringLength(100, ErrorMessage = "La source ne doit pas dépasser 100 caractères.")]
        public string SourceName { get; set; }

        /// <summary>
        /// Montant de la dépense.
        /// </summary>
        /// <example>850.00</example>
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
