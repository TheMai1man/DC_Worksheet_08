
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BankDataWebService.Data;
using BankDataWebService.Models;

namespace BankDataWebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly DBManager _context;

        public TransactionsController(DBManager context)
        {
            _context = context;
        }

        // GET: api/Transactions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransaction()
        {
            if (_context.Transactions == null)
            {
                return Problem("Entity set 'DBManager.Transaction' is null.");
            }
            return await _context.Transactions.ToListAsync();
        }

        // POST: api/Transactions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Transaction>> PostTransaction([FromBody]Transaction transaction)
        {
            if (_context.Accounts == null)
            {
                return Problem("Entity set 'DBManager.Accounts' is null.");
            }
            if (_context.Transactions == null)
            {
                return Problem("Entity set 'DBManager.Transaction' is null.");
            }

            // get the account
            var account = await _context.Accounts.FindAsync(transaction.AcctNo);

            // update account balance
            if (account == null)
            {
                return BadRequest("Account ID does not exist!");
            }
            account.Balance += transaction.Amount;
            _context.Entry(account).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            // insert into transaction table
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTransaction", new { id = transaction.TransactionNum }, transaction);
        }
    }
}