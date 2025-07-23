using System.ComponentModel.DataAnnotations;

namespace BudgetAPI.Models
{
    public class Source
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La nom de la source est obligatoire.")]
        [StringLength(100, ErrorMessage = "La source ne doit pas dépasser 100 caractères.")]
        public string Name { get; set; }
    }
}
