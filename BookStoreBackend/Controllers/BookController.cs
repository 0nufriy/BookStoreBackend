using BookStoreBackend.Data;
using BookStoreBackend.Entities;
using BookStoreBackend.Jwt;
using BookStoreBackend.Migrations;
using BookStoreBackend.Models.AuthController;
using BookStoreBackend.Models.BookController;
using BookStoreBackend.Models.UserController;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Server;
using System.Security.Claims;
using static System.Net.Mime.MediaTypeNames;

namespace BookStoreBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly DataContext _context;

        public BookController(DataContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<List<Book>>> GetBooks()
        {
            var books = await _context.Books.Include(b => b.Gener).ToListAsync();
            return Ok(books);
        }
        [HttpGet("getSome/{count}/{iter}")]
        public async Task<ActionResult<List<Book>>> GetSomeBooks(int count, int iter)
        {
            var books = await _context.Books.Include(b => b.Gener).OrderByDescending(b => b.Id).Skip((iter -1)* count).Take(count).OrderBy(c => c.Id).OrderByDescending(b => b.Id).ToListAsync();
            return Ok(books);
        }
        [HttpGet("genre")]
        public async Task<ActionResult<List<Book>>> GetGenre()
        {
            var genres = await _context.Genre.ToListAsync();
            return Ok(genres);
        }

        [HttpGet("genre/five")]
        public async Task<ActionResult<List<Book>>> GetFiveGenre()
        {
            var genres = await _context.Genre.Take(5).ToListAsync();
            return Ok(genres);
        }

        [HttpGet("getOne/{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            var book = await _context.Books.Include(b => b.Gener).FirstOrDefaultAsync(b => b.Id == id);
            if (book is null) return NotFound("Book not found");
            return Ok(book);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Book>> CreateBook([FromBody]CreateBookDTO createBookDTO)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity is null) return NotFound("User not found");
            var role = identity.Claims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value;
            if (role == "user") return Unauthorized("You are not admin");

            var bookToAdd = new Book()
            {
                Autor = createBookDTO.Autor,
                BookName = createBookDTO.BookName,
                Description = createBookDTO.Description,
                GenreId = createBookDTO.GenreId,
                Image = createBookDTO.Image,
                Isbn = createBookDTO.Isbn,
                Price = createBookDTO.Price,
                Stock = createBookDTO.Stock,
                Cover = createBookDTO.Cover,
                Pagecount = createBookDTO.Pagecount,
                Format = createBookDTO.Format

            };
            _context.Books.Add(bookToAdd);  
            await _context.SaveChangesAsync();
            return Ok(bookToAdd);
        }
        [Authorize]
        [HttpPost("genre")]
        public async Task<ActionResult<Book>> CreateGenre([FromBody] string genreName)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity is null) return NotFound("User not found");
            var role = identity.Claims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value;
            if (role == "user") return Unauthorized("You are not admin");

            var generToAdd = new Genre()
            {
                GenreName = genreName
            };
            _context.Genre.Add(generToAdd);
            await _context.SaveChangesAsync();
            return Ok(generToAdd);
        }
        [Authorize]
        [HttpDelete("genre/{id}")]
        public async Task<ActionResult<Book>> RemoveGenre(int id)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity is null) return NotFound("User not found");
            var role = identity.Claims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value;
            if (role == "user") return Unauthorized("You are not admin");

            var genre = await _context.Genre.FindAsync(id);
            if (genre is null) return NotFound("Genre is not found");
            _context.Genre.Remove(genre);

            await _context.SaveChangesAsync();
            return Ok();
        }

        [Authorize]
        [HttpPut]
        public async Task<ActionResult<Book>> UpdateBook([FromBody] UpdateBookDTO updateBookDTO)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity is null) return NotFound("User not found");
            var role = identity.Claims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value;
            if (role == "user") return Unauthorized("You are not admin");

            var book = await _context.Books.FindAsync(updateBookDTO.Id);
            if (book is null) return NotFound("Book is not found");
            book.Autor = updateBookDTO.Autor;
            book.BookName = updateBookDTO.BookName;
            book.Description = updateBookDTO.Description;
            book.GenreId = updateBookDTO.GenreId;
            book.Image = updateBookDTO.Image;
            book.Isbn = updateBookDTO.Isbn;
            book.Price = updateBookDTO.Price;
            book.Stock = updateBookDTO.Stock;
            book.Cover = updateBookDTO.Cover;
            book.Pagecount = updateBookDTO.Pagecount;
            book.Format = updateBookDTO.Format;

            await _context.SaveChangesAsync();
            return Ok(book);
        }
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Book>> DeleteBook(int id)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity is null) return NotFound("User not found");
            var role = identity.Claims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value;
            if (role == "user") return Unauthorized("You are not admin");

            var book = await _context.Books.FindAsync(id);
            if (book is null) return NotFound("Book is not found");
            _context.Books.Remove(book);

            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
