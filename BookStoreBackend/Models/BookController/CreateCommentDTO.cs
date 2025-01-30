namespace BookStoreBackend.Models.BookController
{
    public class CreateCommentDTO
    {
        public required int BookId { get; set; }
        public required string Message { get; set; }
    }
}
