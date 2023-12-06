using System.ComponentModel.DataAnnotations;

namespace ESHOP.Models
{
    public class Contact
    {
        [Key]
        public int ContactId { get; set; }
        public string Name { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
