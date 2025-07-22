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
    public class OutcomesController : ControllerBase
    {
        private readonly AccountContext _context;

        public OutcomesController(AccountContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Récupère tous les résultats (dépenses).
        /// </summary>
        /// <returns>Liste de toutes les dépenses</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Outcome>>> GetOutcome()
        {
            return await _context.Outcome.ToListAsync();
        }

        /// <summary>
        /// Récupère une dépense par son ID.
        /// </summary>
        /// <param name="id">Identifiant de la dépense</param>
        /// <returns>Dépense correspondante</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Outcome>> GetOutcome(int id)
        {
            var outcome = await _context.Outcome.FindAsync(id);

            if (outcome == null)
            {
                return NotFound();
            }

            return outcome;
        }

        /// <summary>
        /// Modifie une dépense existante.
        /// </summary>
        /// <param name="id">ID de la dépense à modifier</param>
        /// <param name="outcome">Nouvelle valeur de la dépense</param>
        /// <returns>Code HTTP 204 si succès</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOutcome(int id, Outcome outcome)
        {
            if (id != outcome.Id)
            {
                return BadRequest();
            }

            _context.Entry(outcome).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OutcomeExists(id))
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
        /// Crée une nouvelle dépense.
        /// </summary>
        /// <param name="outcome">Objet dépense à créer</param>
        /// <returns>La dépense créée avec son ID</returns>
        [HttpPost]
        public async Task<ActionResult<Outcome>> PostOutcome(Outcome outcome)
        {
            _context.Outcome.Add(outcome);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOutcome", new { id = outcome.Id }, outcome);
        }

        /// <summary>
        /// Supprime une dépense existante.
        /// </summary>
        /// <param name="id">ID de la dépense à supprimer</param>
        /// <returns>Code HTTP 204 si succès</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOutcome(int id)
        {
            var outcome = await _context.Outcome.FindAsync(id);
            if (outcome == null)
            {
                return NotFound();
            }

            _context.Outcome.Remove(outcome);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Calcule le total de toutes les dépenses.
        /// </summary>
        /// <returns>Total des montants des dépenses</returns>
        [HttpGet("total")]
        public async Task<ActionResult<decimal>> GetTotalAmount()
        {
            var total = await _context.Outcome.SumAsync(o => o.Amount);
            return Ok(total);
        }

        /// <summary>
        /// Calcule le total des dépenses pour un mois et une année donnés.
        /// </summary>
        /// <param name="year">Année</param>
        /// <param name="month">Mois</param>
        /// <returns>Total des dépenses pour ce mois</returns>
        [HttpGet("total/month/{year:int}/{month:int}")]
        public async Task<ActionResult<decimal>> GetTotalByMonth(int year, int month)
        {
            var total = await _context.Outcome
                .Where(o => o.DateReceived.Year == year && o.DateReceived.Month == month)
                .SumAsync(o => o.Amount);

            return Ok(total);
        }

        /// <summary>
        /// Calcule le total des dépenses pour une année donnée.
        /// </summary>
        /// <param name="year">Année</param>
        /// <returns>Total des dépenses pour cette année</returns>
        [HttpGet("total/year/{year:int}")]
        public async Task<ActionResult<decimal>> GetTotalByYear(int year)
        {
            var total = await _context.Outcome
                .Where(o => o.DateReceived.Year == year)
                .SumAsync(o => o.Amount);

            return Ok(total);
        }

        /// <summary>
        /// Calcule le total des dépenses entre deux dates.
        /// </summary>
        /// <param name="range">Plage de dates contenant la date de début et de fin</param>
        /// <returns>Total des dépenses sur la période</returns>
        [HttpPost("total/range")]
        public async Task<ActionResult<decimal>> GetTotalByDateRange([FromBody] DateRangeDto range)
        {
            if (range.Start > range.End)
            {
                return BadRequest("La date de début doit être antérieure à la date de fin.");
            }

            var total = await _context.Outcome
                .Where(o => o.DateReceived >= range.Start && o.DateReceived <= range.End)
                .SumAsync(o => o.Amount);

            return Ok(total);
        }

        private bool OutcomeExists(int id)
        {
            return _context.Outcome.Any(e => e.Id == id);
        }
    }
}
