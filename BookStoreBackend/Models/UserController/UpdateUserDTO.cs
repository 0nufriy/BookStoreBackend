namespace BookStoreBackend.Models.UserController
{
    public class UpdateUserDTO
    {
        public required string Login { get; set; }
        public required string Email { get; set; }
        public required string Phone { get; set; }
        public required string Name { get; set; }
    }
}
