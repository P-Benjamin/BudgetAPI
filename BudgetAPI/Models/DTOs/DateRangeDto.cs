using System;
using System.ComponentModel.DataAnnotations;

namespace BudgetAPI.Models.DTOs
{
    public class DateRangeDto
    {
        [Required(ErrorMessage = "La date de début est obligatoire.")]
        [DataType(DataType.Date, ErrorMessage = "Format de date invalide pour Start.")]
        public DateTime Start { get; set; }

        [Required(ErrorMessage = "La date de fin est obligatoire.")]
        [DataType(DataType.Date, ErrorMessage = "Format de date invalide pour End.")]
        public DateTime End { get; set; }
    }
}
