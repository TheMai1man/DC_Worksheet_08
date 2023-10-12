
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BankDataWebService.Data;
using BankDataWebService.Models;

namespace BankDataWebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly DBManager _context;

        public AccountsController(DBManager context)
        {
            _context = context;
        }

        // GET: api/Accounts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Account>>> GetAccounts()
        {
            if (_context.Accounts == null)
            {
                return Problem("Entity set 'DBManager.Accounts'  is null.");
            }
            return await _context.Accounts.ToListAsync();
        }

        // GET: api/Accounts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Account>> GetAccount(uint id)
        {
            if (_context.Accounts == null)
            {
                return Problem("Entity set 'DBManager.Accounts'  is null.");
            }

            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
                return BadRequest("Account ID does not exist!");
            }

            return account;
        }

        // PUT: api/Accounts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAccount(uint id, [FromBody]Account account)
        {
            if (id != account.AcctNo)
            {
                return BadRequest("You cannot alter account IDs!");
            }

            _context.Entry(account).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountExists(id))
                {
                    return BadRequest("Account ID does not exist!");
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        // POST: api/Accounts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Account>> PostAccount([FromBody]Account account)
        {
            if (_context.Accounts == null)
            {
                return Problem("Entity set 'DBManager.Accounts'  is null.");
            }
            if ( await _context.Accounts.FindAsync(account.AcctNo) != null)
            {
                return BadRequest("Account ID already exists!");
            }

            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAccount", new { id = account.AcctNo }, account);
        }

        // DELETE: api/Accounts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(uint id)
        {
            if (_context.Accounts == null)
            {
                return Problem("Entity set 'DBManager.Accounts'  is null.");
            }

            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
                return BadRequest("Account ID does not exist!");
            }

            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool AccountExists(uint id)
        {
            return (_context.Accounts?.Any(e => e.AcctNo == id)).GetValueOrDefault();
        }
    }
}
