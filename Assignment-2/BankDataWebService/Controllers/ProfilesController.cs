
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BankDataWebService.Data;
using BankDataWebService.Models;

namespace BankDataWebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfilesController : ControllerBase
    {
        private readonly DBManager _context;

        public ProfilesController(DBManager context)
        {
            _context = context;
        }

        // GET: api/Profiles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Profile>>> GetProfiles()
        {
            if (_context.Profiles == null)
            {
                return Problem("Entity set 'DBManager.Profiles'  is null.");
            }
          
            return await _context.Profiles.ToListAsync();
        }

        // GET: api/Profiles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Profile>> GetProfile(uint id)
        {
            if (_context.Profiles == null)
            {
                return Problem("Entity set 'DBManager.Profiles'  is null.");
            }
            var profile = await _context.Profiles.FindAsync(id);

            if (profile == null)
            {
                return NotFound();
            }

            return profile;
        }


        // GET: api/Profiles/Name/{name}
        [HttpGet("Name/{name}")]
        public async Task<ActionResult<Profile>> GetProfile(string name)
        {
            if (_context.Profiles == null)
            {
                return Problem("Entity set 'DBManager.Profiles'  is null.");
            }
            var profiles = await _context.Profiles.ToListAsync();
            Profile profile = null;

            foreach(Profile p in profiles)
            {
                if(p.Name.Contains(name))
                {
                    profile = p;
                }
            }

            if (profile == null)
            {
                return NotFound();
            }

            return Ok(profile);
        }


        // PUT: api/Profiles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProfile(uint id, [FromBody]Profile profile)
        {
            if (id != profile.UserID)
            {
                return BadRequest("You cannot alter User IDs!");
            }

            _context.Entry(profile).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProfileExists(id))
                {
                    return BadRequest("Profile ID does not exist!");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Profiles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Profile>> PostProfile([FromBody] Profile profile)
        {
            if (_context.Profiles == null)
            {
                return Problem("Entity set 'DBManager.Profiles'  is null.");
            }
            if (await _context.Accounts.FindAsync(profile.UserID) != null)
            {
                return BadRequest("Profile ID already exists!");
            }

            _context.Profiles.Add(profile);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProfile", new { id = profile.UserID }, profile);
        }

        // DELETE: api/Profiles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProfile(uint id)
        {
            if (_context.Profiles == null)
            {
                return Problem("Entity set 'DBManager.Profiles'  is null.");
            }
            var profile = await _context.Profiles.FindAsync(id);
            if (profile == null)
            {
                return BadRequest("Profile ID does not exist!");
            }

            _context.Profiles.Remove(profile);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool ProfileExists(uint id)
        {
            return (_context.Profiles?.Any(e => e.UserID == id)).GetValueOrDefault();
        }
    }
}
