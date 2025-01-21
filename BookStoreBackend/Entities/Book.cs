using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoreBackend.Entities
{
    public class Book
    {
        [Key]
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
        [ForeignKey("GenreId")]
        public Genre Gener { get; set; }

    }
}
