using BookStoreBackend.Entities;

namespace BookStoreBackend.Models.AuthController
{
    public class UserWithTokenDTO
    {
        public int Id { get; set; }
        public required string Login { get; set; }
        public required string Email { get; set; }
        public required string Phone { get; set; }
        public required string Name { get; set; }
        public required string Role { get; set; }
        public required string Token { get; set; }
        public ICollection<UserAddress> Addresses { get; set; } = new List<UserAddress>();
    }
}
