using System;
using System.ComponentModel.DataAnnotations;

namespace BudgetAPI.Models
{
    public class Outcome
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "L'Id de la source est obligatoire.")]
        public int SourceId { get; set; }

        [Required(ErrorMessage = "Le montant est obligatoire.")]
        [Range(0.01, 1_000_000, ErrorMessage = "Le montant doit être supérieur à 0.")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "La date est obligatoire.")]
        [DataType(DataType.Date, ErrorMessage = "Date invalide.")]
        public DateTime DateReceived { get; set; }

        [Required(ErrorMessage = "La source est obligatoire.")]
        public Source Source { get; set; }
    }
}
