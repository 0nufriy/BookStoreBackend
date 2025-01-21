namespace BookStoreBackend.Models.BookController
{
    public class CreateBookDTO
    {
        public required int GenreId { get; set; }
        public required string BookName { get; set; }
        public required string Autor { get; set; }
        public required string Description { get; set; }
        public int Pagecount { get; set; }
        public string Format { get; set; }
        public required string Isbn { get; set; }
        public string Cover { get; set; }
        public required string Image { get; set; }
        public required int Price { get; set; }
        public required int Stock { get; set; }
    }
}
