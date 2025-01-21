using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoreBackend.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public required string Login { get; set; }
        public required string Password { get; set; }
        public required string Email { get; set; }
        public required string Phone { get; set; }
        public required string Name { get; set; }
        public required string Role { get; set; }

        public ICollection<UserAddress> Addresses { get; set; } = new List<UserAddress>();

    }
}
