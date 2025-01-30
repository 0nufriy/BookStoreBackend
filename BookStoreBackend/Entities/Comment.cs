using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.Design;

namespace BookStoreBackend.Entities
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }
        public required int BookId { get; set; }
        public required int UserId { get; set; }
        public required string Message { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
