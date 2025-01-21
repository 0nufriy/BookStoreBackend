using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoreBackend.Entities
{
    [PrimaryKey(nameof(ReceiptId), nameof(BookId))]
    public class BookReceipt
    {
        public int ReceiptId { get; set; }
        public required int BookId { get; set; }
        public required int Count { get; set; }
        [ForeignKey("BookId")]
        public Book Book { get; set; }
    }
}
