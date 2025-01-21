using System.ComponentModel.DataAnnotations;

namespace BookStoreBackend.Entities
{
    public class Genre
    {
        [Key]
        public int Id { get; set; }
        public required string GenreName { get; set; }
    }
}
