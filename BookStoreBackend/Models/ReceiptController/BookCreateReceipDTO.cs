namespace BookStoreBackend.Models.ReceiptController
{
    public class BookCreateReceipDTO
    {
        public required int BookId { get; set; }
        public required int Count { get; set; }
    }
}
