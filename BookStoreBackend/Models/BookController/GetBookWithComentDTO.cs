using BookStoreBackend.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BookStoreBackend.Models.BookController
{
    public class GetBookWithComentDTO
    {
        public int Id { get; set; }
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
        public Genre Gener { get; set; }
        public ICollection<CommentDTO> Comments { get; set; } = new List<CommentDTO>();
    }
}
