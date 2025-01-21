using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoreBackend.Entities
{
    public class Receipt
    {
        [Key]
        public int Id { get; set; }
        public required int UserId { get; set; }
        public required string Address { get; set; }
        public ICollection<BookReceipt> Books { get; set; } = new List<BookReceipt>();
        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
