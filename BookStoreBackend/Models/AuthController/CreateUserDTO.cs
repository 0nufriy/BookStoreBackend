namespace BookStoreBackend.Models.AuthController
{
    public class CreateUserDTO
    {
        public required string Login { get; set; }
        public required string Password { get; set; }
        public required string Email { get; set; }
        public required string Phone { get; set; }
        public required string Name { get; set; }
    }
}
