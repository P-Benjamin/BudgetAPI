using System;
using System.ComponentModel.DataAnnotations;

namespace BudgetAPI.Models.DTOs
{
    /// <summary>
    /// Représente une plage de dates utilisée pour filtrer les revenus ou les dépenses.
    /// </summary>
    public class DateRangeDto
    {
        /// <summary>
        /// Date de début de la période (incluse).
        /// </summary>
        /// <example>2025-07-01</example>
        [Required(ErrorMessage = "La date de début est obligatoire.")]
        [DataType(DataType.Date, ErrorMessage = "Format de date invalide pour Start.")]
        public DateTime Start { get; set; }

        /// <summary>
        /// Date de fin de la période (incluse).
        /// </summary>
        /// <example>2025-07-31</example>
        [Required(ErrorMessage = "La date de fin est obligatoire.")]
        [DataType(DataType.Date, ErrorMessage = "Format de date invalide pour End.")]
        public DateTime End { get; set; }
    }
}
