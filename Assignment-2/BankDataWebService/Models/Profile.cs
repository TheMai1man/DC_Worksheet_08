using System.ComponentModel.DataAnnotations;

namespace BankDataWebService.Models
{
    public class Profile
    {
        [Key]
        public uint UserID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public uint Phone { get; set; }
        public string Pwd { get; set; }
    }
}
