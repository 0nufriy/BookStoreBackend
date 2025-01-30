namespace BookStoreBackend.Models.BookController
{
    public class CommentDTO
    {
        public required int Id { get; set; }
        public required string UserName { get; set; }
        public required string Message { get; set; }
    }
}
