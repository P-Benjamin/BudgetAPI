using System;
using System.ComponentModel.DataAnnotations;

namespace BudgetAPI.Models
{
    public class Income
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La source est obligatoire.")]
        [StringLength(100, ErrorMessage = "La source ne doit pas dépasser 100 caractères.")]
        public string Source { get; set; }

        [Required(ErrorMessage = "Le montant est obligatoire.")]
        [Range(0.01, 1_000_000, ErrorMessage = "Le montant doit être supérieur à 0.")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "La date est obligatoire.")]
        [DataType(DataType.Date, ErrorMessage = "Date invalide.")]
        public DateTime DateReceived { get; set; }
    }
}
