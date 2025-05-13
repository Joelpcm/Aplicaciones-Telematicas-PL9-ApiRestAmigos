using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Amigos.DataAccessLayer;
using Amigos.Models;

namespace Amigos.Controllers
{
    [Route("api/amigo")]
    [ApiController]
    public class AmigoAPIController : ControllerBase
    {
        private readonly AmigoDBContext _context;

        public AmigoAPIController(AmigoDBContext context)
        {
            _context = context;
        }

        // GET: api/AmigoAPI
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Amigo>>> GetAmigos()
        {
          if (_context.Amigos == null)
          {
              return NotFound();
          }
            return await _context.Amigos.ToListAsync();
        }

        // GET: api/AmigoAPI/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Amigo>> GetAmigo(int id)
        {
          if (_context.Amigos == null)
          {
              return NotFound();
          }
            var amigo = await _context.Amigos.FindAsync(id);

            if (amigo == null)
            {
                return NotFound();
            }

            return amigo;
        }

        // PUT: api/AmigoAPI/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAmigo(int id, Amigo amigo)
        {
            if (id != amigo.ID)
            {
                return BadRequest();
            }

            _context.Entry(amigo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AmigoExists(id))
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

        // PUT: api/AmigoAPI/posicion
        [HttpPut("posicion")]
        public async Task<IActionResult> ActualizarPosicion([FromBody] ActualizacionPosicion actualizacion)
        {
            if (string.IsNullOrWhiteSpace(actualizacion.Name)) 
            { 
                return BadRequest("El nombre es obligatorio.");
            }

            if (_context.Amigos == null)
            {
                return Problem("DataContext \"Amigos\" es nulo");
            }

            var amigo = await _context.Amigos.FirstOrDefaultAsync(a => a.name == actualizacion.Name);

            if (amigo == null)
            {
                return NotFound($"No se encontró un amigo con el nombre '{actualizacion.Name}'.");
            }

            amigo.lati = actualizacion.Lati;
            amigo.longi = actualizacion.Longi;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/AmigoAPI
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Amigo>> PostAmigo(Amigo amigo)
        {
          if (_context.Amigos == null)
          {
              return Problem("Entity set 'AmigoDBContext.Amigos'  is null.");
          }
            _context.Amigos.Add(amigo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAmigo", new { id = amigo.ID }, amigo);
        }

        // DELETE: api/AmigoAPI/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAmigo(int id)
        {
            if (_context.Amigos == null)
            {
                return NotFound();
            }
            var amigo = await _context.Amigos.FindAsync(id);
            if (amigo == null)
            {
                return NotFound();
            }

            _context.Amigos.Remove(amigo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AmigoExists(int id)
        {
            return (_context.Amigos?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
