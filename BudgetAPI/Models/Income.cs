using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BudgetAPI.Models
{
    /// <summary>
    /// Représente un revenu saisi par un utilisateur.
    /// </summary>
    public class Income
    {
        /// <summary>
        /// Identifiant unique du revenu.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Identifiant de la source associée (ex : Salaire, Bourse, etc.).
        /// </summary>
        [Required(ErrorMessage = "L'Id de la source est obligatoire.")]
        public int SourceId { get; set; }

        /// <summary>
        /// Montant du revenu.
        /// </summary>
        [Required(ErrorMessage = "Le montant est obligatoire.")]
        [Range(0.01, 1_000_000, ErrorMessage = "Le montant doit être supérieur à 0.")]
        public decimal Amount { get; set; }

        /// <summary>
        /// Date à laquelle le revenu a été perçu.
        /// </summary>
        [Required(ErrorMessage = "La date est obligatoire.")]
        [DataType(DataType.Date, ErrorMessage = "Date invalide.")]
        public DateTime DateReceived { get; set; }

        /// <summary>
        /// Objet Source associé à ce revenu.
        /// </summary>
        [Required(ErrorMessage = "La source est obligatoire.")]
        [ValidateNever] // Empêche la validation récursive par ASP.NET MVC
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Source Source { get; set; }
    }
}
