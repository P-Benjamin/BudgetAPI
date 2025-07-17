using BudgetAPI.Models;
using BudgetAPI.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        // GET: api/Incomes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Income>>> GetIncome()
        {
            return await _context.Income.ToListAsync();
        }

        // GET: api/Incomes/5
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

        // PUT: api/Incomes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
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

        // POST: api/Incomes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Income>> PostIncome(Income income)
        {
            _context.Income.Add(income);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetIncome", new { id = income.Id }, income);
        }

        // DELETE: api/Incomes/5
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

        private bool IncomeExists(int id)
        {
            return _context.Income.Any(e => e.Id == id);
        }

        // GET: api/Incomes
        [HttpGet("total")]
        public async Task<ActionResult<string>> GetTotalIncome()
        {
            var total = await _context.Income.SumAsync(i => i.Amount);
            return Ok(total);
            
        }

        // GET: api/Incomes/total/month/{year}/{month}
        [HttpGet("total/month/{year:int}/{month:int}")]
        public async Task<ActionResult<decimal>> GetTotalByMonth(int year, int month)
        {
            var total = await _context.Income
                .Where(o => o.DateReceived.Year == year && o.DateReceived.Month == month)
                .SumAsync(o => o.Amount);

            return Ok(total);
        }

        // GET: api/Incomes/total/year/{year}
        [HttpGet("total/year/{year:int}")]
        public async Task<ActionResult<decimal>> GetTotalByYear(int year)
        {
            var total = await _context.Income
                .Where(o => o.DateReceived.Year == year)
                .SumAsync(o => o.Amount);

            return Ok(total);
        }

        // POST: api/Incomes/total/range
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
    }
}
