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
        /// Récupère toutes les dépenses.
        /// </summary>
        /// <remarks>
        /// Exemple de réponse :
        ///
        ///     [
        ///       {
        ///         "id": 1,
        ///         "sourceName": "Loyer",
        ///         "amount": 800,
        ///         "dateReceived": "2025-07-24"
        ///       }
        ///     ]
        /// </remarks>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<OutcomeViewDTO>), 200)]
        public async Task<ActionResult<IEnumerable<OutcomeViewDTO>>> GetOutcome()
        {
            var outcomes = await _context.Outcome
                .Include(o => o.Source)
                .Select(o => new OutcomeViewDTO
                {
                    Id = o.Id,
                    SourceId = o.SourceId,
                    SourceName = o.Source.Name,
                    Amount = o.Amount,
                    DateReceived = o.DateReceived
                })
                .ToListAsync();

            return Ok(outcomes);
        }

        /// <summary>
        /// Récupère une dépense par son ID.
        /// </summary>
        /// <param name="id">Identifiant unique de la dépense</param>
        /// <remarks>
        /// Exemple : GET /api/outcomes/1
        /// </remarks>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(OutcomeViewDTO), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<OutcomeViewDTO>> GetOutcome(int id)
        {
            var outcome = await _context.Outcome
                .Include(o => o.Source)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (outcome == null)
            {
                return NotFound();
            }

            var dto = new OutcomeViewDTO
            {
                Id = outcome.Id,
                SourceId = outcome.SourceId,
                SourceName = outcome.Source.Name,
                Amount = outcome.Amount,
                DateReceived = outcome.DateReceived
            };

            return Ok(dto);
        }

        /// <summary>
        /// Met à jour une dépense existante.
        /// </summary>
        /// <param name="id">ID de la dépense à modifier</param>
        /// <param name="dto">Données mises à jour de la dépense</param>
        /// <remarks>
        /// Exemple de requête :
        ///
        ///     PUT /api/outcomes/1
        ///     {
        ///       "id": 1,
        ///       "sourceId": 2,
        ///       "amount": 900,
        ///       "dateReceived": "2025-07-25"
        ///     }
        /// </remarks>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(OutcomeViewDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<OutcomeViewDTO>> PutOutcome(int id, OutcomeDTO dto)
        {
            if (id != dto.Id)
            {
                return BadRequest();
            }

            var existing = await _context.Outcome.FindAsync(id);
            if (existing == null)
            {
                return NotFound();
            }

            existing.SourceId = dto.SourceId;
            existing.Amount = dto.Amount;
            existing.DateReceived = dto.DateReceived;

            await _context.SaveChangesAsync();

            var updated = await _context.Outcome.Include(o => o.Source).FirstOrDefaultAsync(o => o.Id == id);

            var viewDto = new OutcomeViewDTO
            {
                Id = updated.Id,
                SourceId = updated.SourceId,
                SourceName = updated.Source?.Name,
                Amount = updated.Amount,
                DateReceived = updated.DateReceived
            };

            return Ok(viewDto);
        }

        /// <summary>
        /// Crée une nouvelle dépense.
        /// </summary>
        /// <param name="dto">Objet dépense à créer</param>
        /// <remarks>
        /// Exemple de requête :
        ///
        ///     {
        ///       "sourceId": 2,
        ///       "amount": 600,
        ///       "dateReceived": "2025-07-24"
        ///     }
        /// </remarks>
        [HttpPost]
        [ProducesResponseType(typeof(OutcomeViewDTO), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<OutcomeViewDTO>> PostOutcome(OutcomeDTO dto)
        {
            var outcome = new Outcome
            {
                SourceId = dto.SourceId,
                Amount = dto.Amount,
                DateReceived = dto.DateReceived
            };

            _context.Outcome.Add(outcome);
            await _context.SaveChangesAsync();

            var fullOutcome = await _context.Outcome.Include(o => o.Source).FirstOrDefaultAsync(o => o.Id == outcome.Id);

            var viewDto = new OutcomeViewDTO
            {
                Id = fullOutcome.Id,
                SourceId = fullOutcome.SourceId,
                SourceName = fullOutcome.Source?.Name,
                Amount = fullOutcome.Amount,
                DateReceived = fullOutcome.DateReceived
            };

            return CreatedAtAction(nameof(GetOutcome), new { id = outcome.Id }, viewDto);
        }

        /// <summary>
        /// Supprime une dépense.
        /// </summary>
        /// <param name="id">ID de la dépense à supprimer</param>
        /// <remarks>
        /// Exemple : DELETE /api/outcomes/1
        /// </remarks>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
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
        /// <remarks>
        /// Exemple : GET /api/outcomes/total
        /// </remarks>
        [HttpGet("total")]
        [ProducesResponseType(typeof(decimal), 200)]
        public async Task<ActionResult<decimal>> GetTotalAmount()
        {
            var total = await _context.Outcome.SumAsync(o => o.Amount);
            return Ok(total);
        }

        /// <summary>
        /// Calcule le total des dépenses pour un mois donné.
        /// </summary>
        /// <remarks>
        /// Exemple : GET /api/outcomes/total/month/2025/7
        /// </remarks>
        [HttpGet("total/month/{year:int}/{month:int}")]
        [ProducesResponseType(typeof(decimal), 200)]
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
        /// <remarks>
        /// Exemple : GET /api/outcomes/total/year/2025
        /// </remarks>
        [HttpGet("total/year/{year:int}")]
        [ProducesResponseType(typeof(decimal), 200)]
        public async Task<ActionResult<decimal>> GetTotalByYear(int year)
        {
            var total = await _context.Outcome
                .Where(o => o.DateReceived.Year == year)
                .SumAsync(o => o.Amount);

            return Ok(total);
        }

        /// <summary>
        /// Calcule le total des dépenses sur une période donnée.
        /// </summary>
        /// <remarks>
        /// Exemple de requête :
        ///
        ///     {
        ///       "start": "2025-07-01",
        ///       "end": "2025-07-31"
        ///     }
        /// </remarks>
        [HttpPost("total/range")]
        [ProducesResponseType(typeof(decimal), 200)]
        [ProducesResponseType(400)]
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

        /// <summary>
        /// Récupère toutes les dépenses associées à une source spécifique.
        /// </summary>
        /// <param name="sourceId">ID de la source</param>
        /// <returns>Liste des dépenses liées à cette source</returns>
        [HttpGet("by-source/{sourceId}")]
        [ProducesResponseType(typeof(IEnumerable<OutcomeViewDTO>), 200)]
        public async Task<ActionResult<IEnumerable<OutcomeViewDTO>>> GetOutcomesBySource(int sourceId)
        {
            var outcomes = await _context.Outcome
                .Include(o => o.Source)
                .Where(o => o.SourceId == sourceId)
                .Select(o => new OutcomeViewDTO
                {
                    Id = o.Id,
                    SourceName = o.Source.Name,
                    Amount = o.Amount,
                    DateReceived = o.DateReceived
                })
                .ToListAsync();

            return Ok(outcomes);
        }

        private bool OutcomeExists(int id)
        {
            return _context.Outcome.Any(e => e.Id == id);
        }
    }
}
