namespace BookStoreBackend.Models.ReceiptController
{
    public class BookInReceiptDTO
    {
        public int Id { get; set; }
        public required string BookName { get; set; }
        public required string Autor { get; set; }
        public required string Image { get; set; }
        public required int Price { get; set; }
        public required int Count { get; set; }
    }
}
