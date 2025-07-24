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
        /// <remarks>
        /// Exemple de réponse :
        ///
        ///     [
        ///       {
        ///         "id": 1,
        ///         "sourceName": "Salaire",
        ///         "amount": 1500,
        ///         "dateReceived": "2025-07-01T00:00:00"
        ///       }
        ///     ]
        /// </remarks>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<IncomeViewDTO>), 200)]
        public async Task<ActionResult<IEnumerable<IncomeViewDTO>>> GetIncome()
        {
            var result = await _context.Income
                .Include(i => i.Source)
                .Select(i => new IncomeViewDTO
                {
                    Id = i.Id,
                    SourceName = i.Source.Name,
                    Amount = i.Amount,
                    DateReceived = i.DateReceived
                })
                .ToListAsync();

            return result;
        }

        /// <summary>
        /// Récupère un revenu spécifique.
        /// </summary>
        /// <param name="id">ID du revenu</param>
        /// <remarks>
        /// Exemple de réponse :
        ///
        ///     {
        ///       "id": 2,
        ///       "sourceName": "Investissement",
        ///       "amount": 300,
        ///       "dateReceived": "2025-07-05T00:00:00"
        ///     }
        /// </remarks>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(IncomeViewDTO), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IncomeViewDTO>> GetIncome(int id)
        {
            var income = await _context.Income
                .Include(i => i.Source)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (income == null)
            {
                return NotFound();
            }

            var dto = new IncomeViewDTO
            {
                Id = income.Id,
                SourceName = income.Source?.Name,
                Amount = income.Amount,
                DateReceived = income.DateReceived
            };

            return dto;
        }

        /// <summary>
        /// Met à jour un revenu existant.
        /// </summary>
        /// <param name="id">ID du revenu</param>
        /// <param name="income">Revenu mis à jour</param>
        /// <remarks>
        /// Exemple de requête :
        ///
        ///     {
        ///       "id": 4,
        ///       "sourceId": 2,
        ///       "amount": 2200,
        ///       "dateReceived": "2025-07-24"
        ///     }
        /// </remarks>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(IncomeViewDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IncomeViewDTO>> PutIncome(int id, IncomeDTO income)
        {
            if (id != income.Id)
            {
                return BadRequest();
            }

            var existingIncome = await _context.Income.FindAsync(id);
            if (existingIncome == null)
            {
                return NotFound();
            }

            existingIncome.SourceId = income.SourceId;
            existingIncome.Amount = income.Amount;
            existingIncome.DateReceived = income.DateReceived;

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

            var updatedIncome = await _context.Income
                .Include(i => i.Source)
                .FirstOrDefaultAsync(i => i.Id == id);

            var dto = new IncomeViewDTO
            {
                Id = updatedIncome.Id,
                SourceName = updatedIncome.Source?.Name,
                Amount = updatedIncome.Amount,
                DateReceived = updatedIncome.DateReceived
            };

            return Ok(dto);
        }

        /// <summary>
        /// Crée un nouveau revenu.
        /// </summary>
        /// <param name="incomeDto">Revenu à ajouter</param>
        /// <remarks>
        /// Exemple de requête :
        ///
        ///     {
        ///       "sourceId": 1,
        ///       "amount": 2000,
        ///       "dateReceived": "2025-07-24"
        ///     }
        /// </remarks>
        [HttpPost]
        [ProducesResponseType(typeof(IncomeViewDTO), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<IncomeViewDTO>> PostIncome(IncomeDTO incomeDto)
        {
            var income = new Income
            {
                SourceId = incomeDto.SourceId,
                Amount = incomeDto.Amount,
                DateReceived = incomeDto.DateReceived
            };

            _context.Income.Add(income);
            await _context.SaveChangesAsync();

            var fullIncome = await _context.Income
                .Include(i => i.Source)
                .FirstOrDefaultAsync(i => i.Id == income.Id);

            var dto = new IncomeViewDTO
            {
                Id = fullIncome.Id,
                SourceName = fullIncome.Source?.Name,
                Amount = fullIncome.Amount,
                DateReceived = fullIncome.DateReceived
            };

            return CreatedAtAction(nameof(GetIncome), new { id = dto.Id }, dto);
        }

        /// <summary>
        /// Supprime un revenu.
        /// </summary>
        /// <param name="id">ID du revenu</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
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
        [HttpGet("total")]
        [ProducesResponseType(typeof(decimal), 200)]
        public async Task<ActionResult<decimal>> GetTotalIncome()
        {
            var total = await _context.Income.SumAsync(i => i.Amount);
            return Ok(total);
        }

        /// <summary>
        /// Calcule le revenu total pour un mois donné.
        /// </summary>
        /// <param name="year">Année</param>
        /// <param name="month">Mois</param>
        [HttpGet("total/month/{year:int}/{month:int}")]
        [ProducesResponseType(typeof(decimal), 200)]
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
        [HttpGet("total/year/{year:int}")]
        [ProducesResponseType(typeof(decimal), 200)]
        public async Task<ActionResult<decimal>> GetTotalByYear(int year)
        {
            var total = await _context.Income
                .Where(o => o.DateReceived.Year == year)
                .SumAsync(o => o.Amount);

            return Ok(total);
        }

        /// <summary>
        /// Calcule le revenu total entre deux dates.
        /// </summary>
        /// <param name="range">Date de début et de fin</param>
        [HttpPost("total/range")]
        [ProducesResponseType(typeof(decimal), 200)]
        [ProducesResponseType(400)]
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

        /// <summary>
        /// Vérifie si un revenu existe.
        /// </summary>
        private bool IncomeExists(int id)
        {
            return _context.Income.Any(e => e.Id == id);
        }
    }
}
