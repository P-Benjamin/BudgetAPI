using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BudgetAPI.Models
{
    /// <summary>
    /// Représente une dépense enregistrée par l'utilisateur.
    /// </summary>
    public class Outcome
    {
        /// <summary>
        /// Identifiant unique de la dépense.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Identifiant de la source liée à cette dépense (ex : Loyer, Abonnement...).
        /// </summary>
        [Required(ErrorMessage = "L'Id de la source est obligatoire.")]
        public int SourceId { get; set; }

        /// <summary>
        /// Montant de la dépense.
        /// </summary>
        [Required(ErrorMessage = "Le montant est obligatoire.")]
        [Range(0.01, 1_000_000, ErrorMessage = "Le montant doit être supérieur à 0.")]
        public decimal Amount { get; set; }

        /// <summary>
        /// Date à laquelle la dépense a été effectuée.
        /// </summary>
        [Required(ErrorMessage = "La date est obligatoire.")]
        [DataType(DataType.Date, ErrorMessage = "Date invalide.")]
        public DateTime DateReceived { get; set; }

        /// <summary>
        /// Objet Source représentant la catégorie ou origine de la dépense.
        /// </summary>
        [Required(ErrorMessage = "La source est obligatoire.")]
        [ValidateNever] // Ne pas valider les propriétés internes de Source
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Source Source { get; set; }
    }
}
