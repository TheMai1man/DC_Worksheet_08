using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankDataWebService.Models
{
    public class Account
    {
        public int Balance { get; set; }
        [Key]
        public uint AcctNo { get; set; }
        public uint UserID { get; set; }
    }
}
