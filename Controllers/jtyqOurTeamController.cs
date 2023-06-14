using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using jtyq.Models;

namespace jtyq.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OurTeamController : ControllerBase
    {
        private readonly jtyqContext _context;

        public OurTeamController(jtyqContext context)
        {
            _context = context;
        }

        // GET: api/jtyqOurTeam
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OurTeam>>> GetOurTeam()
        {
          await _context.Connection.OpenAsync();
          var query = new OurTeamQuery(_context);
          var result = await query.AllTeamAsync();

          return new OkObjectResult(result);
        }

        // GET: api/jtyqOurTeam/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OurTeam>> GetOurTeamMember(int id)
        {
          await _context.Connection.OpenAsync();
          var query = new OurTeamQuery(_context);
          var result = await query.FindOneAsync(id);

            if (result == null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(result);
        }

        // PUT: api/jtyqOurTeam/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOurTeam(int id, [FromBody]OurTeam ourTeam)
        {
            await _context.Connection.OpenAsync();

            var query = new OurTeamQuery(_context);
            var result = await query.FindOneAsync(id);

            if (result is null)
                return new NotFoundResult();
            
            result.Name = ourTeam.Name;
            result.Description = ourTeam.Description;
            await result.UpdateAsync();
            return new OkObjectResult(result);
        }

        // POST: api/jtyqOurTeam
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<OurTeam>> PostOurTeam([FromBody]OurTeam ourTeam)
        {
          await _context.Connection.OpenAsync();
          ourTeam.Db = _context;
          await ourTeam.InsertAsync();
          return new OkObjectResult(ourTeam);
        }

        // DELETE: api/jtyqOurTeam/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOurTeam(int id)
        {
            await _context.Connection.OpenAsync();
            var query = new OurTeamQuery(_context);
            var result = await query.FindOneAsync(id);
            if (result is null)
                return new NotFoundResult();
            await result.DeleteAsync();
            return new OkResult();
        }

        private bool OurTeamExists(int id)
        {
            return (_context.OurTeam?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
