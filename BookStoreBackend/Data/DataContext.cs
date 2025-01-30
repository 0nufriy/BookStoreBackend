using BookStoreBackend.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookStoreBackend.Data
{
    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions<DataContext> options) : base(options) 
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserAddress> Addresses { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Receipt> Receipt { get; set; }
        public DbSet<Genre> Genre { get; set; }
        public DbSet<Comment> Comment { get; set; }

    }
}
