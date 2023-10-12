using System.ComponentModel.DataAnnotations;

namespace BankDataWebService.Models
{
    public class Transaction
    {
        public int Amount { get; set; }
        [Key]
        public uint TransactionNum { get; set; }
        public uint AcctNo { get; set; }
    }
}