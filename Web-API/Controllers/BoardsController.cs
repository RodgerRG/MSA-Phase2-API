using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_API.Data;
using Web_API.DTOs;
using Web_API.Models;

namespace Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BoardsController : ControllerBase
    {
        private readonly ApiContext _context;

        public BoardsController(ApiContext context)
        {
            _context = context;
        }

        // GET: api/Boards
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BoardListDTO>>> GetBoards()
        {
            List<BoardListDTO> ids = new List<BoardListDTO>();
            List<Board> boards = await _context.leagues.ToListAsync();
            foreach(Board b in boards)
            {
                BoardListDTO simpleBoard = new BoardListDTO();
                simpleBoard.boardId = b.boardId;
                simpleBoard.boardName = b.boardName;
                ids.Add(simpleBoard);
            }
            return ids;
        }

        // GET: api/Boards/5
        [HttpGet("{boardId}")]
        public async Task<ActionResult<BoardDTO>> GetBoard(int boardId)
        {
            //Eagerly load in all the info we need.
            var board = await _context.leagues.Include(b => b.jobs)
                .ThenInclude(j => j.poster)
                .ThenInclude(p => p.user)
                .Where(b => b.boardId == boardId).FirstOrDefaultAsync();

            if (board == null)
            {
                return NotFound();
            }

            var boardDTO = new BoardDTO();
            boardDTO.boardId = board.boardId;
            boardDTO.boardName = board.boardName;
            boardDTO.ownerId = board.ownerId;
            boardDTO.jobs = JobDTO.convertJobs(board.jobs);

            return boardDTO;
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
        public async Task<ActionResult<BoardDTO>> AddBoard([FromBody] BoardDTO newBoard)
        {
            Board board = new Board();
            User owner = await _context.users.FindAsync(newBoard.ownerId);
            board.ownerId = owner.Id;
            board.owner = owner;
            board.boardName = newBoard.boardName;
            //I'm probably not going to have enough time to get this working
            board.location = "";

            UserBoard userBoard = new UserBoard();
            userBoard.boardId = board.boardId;
            userBoard.userId = owner.Id;
            userBoard.board = board;
            userBoard.user = owner;
            userBoard.hasJob = false;
            userBoard.rep = 0;

            _context.leagues.Add(board);
            _context.userBoard.Add(userBoard);
            await _context.SaveChangesAsync();

            BoardDTO dto = new BoardDTO();
            dto.boardId = board.boardId;
            dto.ownerId = owner.Id;
            dto.boardName = board.boardName;
            dto.jobs = JobDTO.convertJobs(board.jobs);

            return CreatedAtAction("GetBoard", new { boardId = board.boardId }, dto);
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
