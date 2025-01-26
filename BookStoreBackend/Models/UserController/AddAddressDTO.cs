namespace BookStoreBackend.Models.UserController
{
    public class AddAddressDTO
    {
        public required string AdressName { get; set; }
        public required string City { get; set; }
        public required string Street { get; set; }
        public required string House { get; set; }
        public required string PostalCode { get; set; }
    }
}
