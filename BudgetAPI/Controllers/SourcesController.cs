using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BudgetAPI.Models;
using Microsoft.AspNetCore.Authorization;

namespace BudgetAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SourcesController : ControllerBase
    {
        private readonly AccountContext _context;

        public SourcesController(AccountContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Récupère toutes les sources disponibles.
        /// </summary>
        /// <remarks>
        /// Exemple : GET /api/sources
        /// </remarks>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Source>), 200)]
        public async Task<ActionResult<IEnumerable<Source>>> GetSource()
        {
            return await _context.Source.ToListAsync();
        }

        /// <summary>
        /// Récupère une source par son identifiant.
        /// </summary>
        /// <param name="id">ID de la source</param>
        /// <remarks>
        /// Exemple : GET /api/sources/5
        /// </remarks>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Source), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Source>> GetSource(int id)
        {
            var source = await _context.Source.FindAsync(id);

            if (source == null)
            {
                return NotFound();
            }

            return source;
        }

        /// <summary>
        /// Met à jour une source existante.
        /// </summary>
        /// <param name="id">ID de la source</param>
        /// <param name="source">Objet source à mettre à jour</param>
        /// <remarks>
        /// Exemple :
        /// PUT /api/sources/3
        /// {
        ///   "id": 3,
        ///   "name": "Investissements"
        /// }
        /// </remarks>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> PutSource(int id, Source source)
        {
            if (id != source.Id)
            {
                return BadRequest();
            }

            _context.Entry(source).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SourceExists(id))
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
        /// Crée une nouvelle source.
        /// </summary>
        /// <param name="source">Objet source à créer</param>
        /// <remarks>
        /// Exemple :
        /// POST /api/sources
        /// {
        ///   "name": "Freelance"
        /// }
        /// </remarks>
        [HttpPost]
        [ProducesResponseType(typeof(Source), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Source>> PostSource(Source source)
        {
            _context.Source.Add(source);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSource", new { id = source.Id }, source);
        }

        /// <summary>
        /// Supprime une source.
        /// </summary>
        /// <param name="id">ID de la source à supprimer</param>
        /// <remarks>
        /// Exemple : DELETE /api/sources/3
        /// Renvoie 400 si la source est liée à des revenus ou dépenses.
        /// </remarks>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> DeleteSource(int id)
        {
            var source = await _context.Source.FindAsync(id);
            if (source == null)
            {
                return NotFound();
            }

            bool isUsed = await _context.Income.AnyAsync(i => i.SourceId == id)
                           || await _context.Outcome.AnyAsync(o => o.SourceId == id);

            if (isUsed)
            {
                return BadRequest("Impossible de supprimer une source liée à des revenus ou des dépenses.");
            }

            _context.Source.Remove(source);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SourceExists(int id)
        {
            return _context.Source.Any(e => e.Id == id);
        }
    }
}