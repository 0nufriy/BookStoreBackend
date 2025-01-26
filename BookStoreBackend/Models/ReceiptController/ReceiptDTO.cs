using BookStoreBackend.Entities;
using BookStoreBackend.Models.Enum;
using BookStoreBackend.Models.UserController;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoreBackend.Models.ReceiptController
{
    public class ReceiptDTO
    {
        public int Id { get; set; }
        public required int UserId { get; set; }
        public required string Address { get; set; }
        public required StatusEnum Status { get; set; }
        public ICollection<BookReceipt> Books { get; set; } = new List<BookReceipt>();
        public UserDataDTO User { get; set; }
    }
}
