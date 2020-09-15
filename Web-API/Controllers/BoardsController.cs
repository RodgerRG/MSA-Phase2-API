using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_API.Data;
using Web_API.Models;

namespace Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoardsController : ControllerBase
    {
        private readonly ApiContext _context;

        public BoardsController(ApiContext context)
        {
            _context = context;
        }

        // GET: api/Boards
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Board>>> Getleagues()
        {
            return await _context.leagues.ToListAsync();
        }

        // GET: api/Boards/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Board>> GetBoard(int id)
        {
            var board = await _context.leagues.FindAsync(id);

            if (board == null)
            {
                return NotFound();
            }

            return board;
        }

        // PUT: api/Boards/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBoard(int id, Board board)
        {
            if (id != board.boardId)
            {
                return BadRequest();
            }

            _context.Entry(board).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BoardExists(id))
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

        // POST: api/Boards
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Board>> PostBoard(Board board)
        {
            _context.leagues.Add(board);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBoard", new { id = board.boardId }, board);
        }

        // DELETE: api/Boards/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Board>> DeleteBoard(int id)
        {
            var board = await _context.leagues.FindAsync(id);
            if (board == null)
            {
                return NotFound();
            }

            _context.leagues.Remove(board);
            await _context.SaveChangesAsync();

            return board;
        }

        private bool BoardExists(int id)
        {
            return _context.leagues.Any(e => e.boardId == id);
        }
    }
}
