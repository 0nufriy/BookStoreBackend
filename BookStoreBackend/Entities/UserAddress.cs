using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoreBackend.Entities
{
    public class UserAddress
    {
        [Key]
        public int Id { get; set; }
        public required string AdressName { get; set; }
        public required string Adress { get; set; }
        public int UserId { get; set; }
    }
}
