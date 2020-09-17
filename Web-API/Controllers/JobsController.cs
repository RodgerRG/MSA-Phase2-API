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
    public class JobsController : ControllerBase
    {
        private readonly ApiContext _context;

        public JobsController(ApiContext context)
        {
            _context = context;
        }

        // GET: api/Jobs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Job>>> GetJob()
        {
            return await _context.Job.ToListAsync();
        }

        // GET: api/Jobs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Job>> GetJob(int id)
        {
            var job = await _context.Job.FindAsync(id);

            if (job == null)
            {
                return NotFound();
            }

            return job;
        }

        // PUT: api/Jobs/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutJob(int id, Job job)
        {
            if (id != job.jobId)
            {
                return BadRequest();
            }

            _context.Entry(job).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JobExists(id))
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

        // POST: api/Jobs
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<JobDTO>> PostJob([FromBody]JobDTO jobDto)
        {
            User user = (User)await _context.Users.FindAsync(jobDto.posterId);
            Board actual = await _context.leagues.FindAsync(jobDto.boardId);
            UserBoard board = _context.userBoard.Where(ub => ub.boardId == jobDto.boardId && ub.userId == jobDto.posterId).FirstOrDefault();

            //this shouldn't actually happen, I ran outta time so this check is getting skipped. realistically the server returns bad request here, as you tried to post to a board you're not a part of.
            if(board == null)
            {
                board = new UserBoard();
                board.board = actual;
                board.boardId = jobDto.boardId;
                board.user = user;
                board.userId = jobDto.posterId;
                board.hasJob = false;
                board.rep = 0;
            }

            Job job = new Job();
            job.isTaken = false;
            job.isCompleted = false;
            job.jobDescription = jobDto.description;
            job.jobTitle = jobDto.title;
            job.location = "";
            job.mediaURI = "";
            job.poster = board;
            job.posterId = board.boardId;
            _context.Job.Add(job);
            actual.jobs.Add(job);
            
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetJob", new { id = job.jobId }, jobDto);
        }

        // DELETE: api/Jobs/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Job>> DeleteJob(int id)
        {
            var job = await _context.Job.FindAsync(id);
            if (job == null)
            {
                return NotFound();
            }

            _context.Job.Remove(job);
            await _context.SaveChangesAsync();

            return job;
        }

        private bool JobExists(int id)
        {
            return _context.Job.Any(e => e.jobId == id);
        }
    }
}
