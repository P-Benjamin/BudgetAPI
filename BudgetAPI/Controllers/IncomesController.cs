using BudgetAPI.Models;
using BudgetAPI.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BudgetAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class IncomesController : ControllerBase
    {
        private readonly AccountContext _context;

        public IncomesController(AccountContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Récupère la liste de tous les revenus.
        /// </summary>
        /// <returns>Liste des revenus</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Income>>> GetIncome()
        {
            return await _context.Income.ToListAsync();
        }

        /// <summary>
        /// Récupère un revenu spécifique selon son ID.
        /// </summary>
        /// <param name="id">Identifiant du revenu</param>
        /// <returns>Revenu correspondant</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Income>> GetIncome(int id)
        {
            var income = await _context.Income.FindAsync(id);
            if (income == null)
            {
                return NotFound();
            }
            return income;
        }

        /// <summary>
        /// Met à jour un revenu existant.
        /// </summary>
        /// <param name="id">ID du revenu à modifier</param>
        /// <param name="income">Objet revenu modifié</param>
        /// <returns>Aucune réponse si succès</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIncome(int id, Income income)
        {
            if (id != income.Id)
            {
                return BadRequest();
            }

            _context.Entry(income).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IncomeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Crée un nouveau revenu.
        /// </summary>
        /// <param name="income">Objet revenu à ajouter</param>
        /// <returns>Le revenu créé</returns>
        [HttpPost]
        public async Task<ActionResult<Income>> PostIncome(Income income)
        {
            _context.Income.Add(income);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetIncome", new { id = income.Id }, income);
        }

        /// <summary>
        /// Supprime un revenu existant.
        /// </summary>
        /// <param name="id">ID du revenu à supprimer</param>
        /// <returns>Aucune réponse si succès</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIncome(int id)
        {
            var income = await _context.Income.FindAsync(id);
            if (income == null)
            {
                return NotFound();
            }

            _context.Income.Remove(income);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Calcule le revenu total.
        /// </summary>
        /// <returns>Total des revenus</returns>
        [HttpGet("total")]
        public async Task<ActionResult<string>> GetTotalIncome()
        {
            var total = await _context.Income.SumAsync(i => i.Amount);
            return Ok(total);
        }

        /// <summary>
        /// Calcule le revenu total pour un mois et une année donnés.
        /// </summary>
        /// <param name="year">Année</param>
        /// <param name="month">Mois</param>
        /// <returns>Total pour ce mois</returns>
        [HttpGet("total/month/{year:int}/{month:int}")]
        public async Task<ActionResult<decimal>> GetTotalByMonth(int year, int month)
        {
            var total = await _context.Income
                .Where(o => o.DateReceived.Year == year && o.DateReceived.Month == month)
                .SumAsync(o => o.Amount);

            return Ok(total);
        }

        /// <summary>
        /// Calcule le revenu total pour une année donnée.
        /// </summary>
        /// <param name="year">Année</param>
        /// <returns>Total pour cette année</returns>
        [HttpGet("total/year/{year:int}")]
        public async Task<ActionResult<decimal>> GetTotalByYear(int year)
        {
            var total = await _context.Income
                .Where(o => o.DateReceived.Year == year)
                .SumAsync(o => o.Amount);

            return Ok(total);
        }

        /// <summary>
        /// Calcule le revenu total sur une plage de dates donnée.
        /// </summary>
        /// <param name="range">Objet contenant les dates de début et de fin</param>
        /// <returns>Total pour la période spécifiée</returns>
        [HttpPost("total/range")]
        public async Task<ActionResult<decimal>> GetTotalByDateRange([FromBody] DateRangeDto range)
        {
            if (range.Start > range.End)
            {
                return BadRequest("La date de début doit être antérieure à la date de fin.");
            }

            var total = await _context.Income
                .Where(o => o.DateReceived >= range.Start && o.DateReceived <= range.End)
                .SumAsync(o => o.Amount);

            return Ok(total);
        }

        private bool IncomeExists(int id)
        {
            return _context.Income.Any(e => e.Id == id);
        }
    }
}
