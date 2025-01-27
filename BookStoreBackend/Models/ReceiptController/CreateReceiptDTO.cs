using BookStoreBackend.Entities;

namespace BookStoreBackend.Models.ReceiptController
{
    public class CreateReceiptDTO
    {
        public required int AddressId { get; set; }

        public required List<BookCreateReceipDTO> Books { get; set; }
    }
}
