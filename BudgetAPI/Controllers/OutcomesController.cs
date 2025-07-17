using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BudgetAPI.Models;

namespace BudgetAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OutcomesController : ControllerBase
    {
        private readonly AccountContext _context;

        public OutcomesController(AccountContext context)
        {
            _context = context;
        }

        // GET: api/Outcomes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Outcome>>> GetOutcome()
        {
            return await _context.Outcome.ToListAsync();
        }

        // GET: api/Outcomes/5
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

        // PUT: api/Outcomes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
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

        // POST: api/Outcomes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Outcome>> PostOutcome(Outcome outcome)
        {
            _context.Outcome.Add(outcome);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOutcome", new { id = outcome.Id }, outcome);
        }

        // DELETE: api/Outcomes/5
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

        private bool OutcomeExists(int id)
        {
            return _context.Outcome.Any(e => e.Id == id);
        }
    }
}
