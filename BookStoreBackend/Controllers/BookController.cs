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
using System.Reflection.Metadata;
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

        [HttpGet("getCataloge/")]
        public async Task<ActionResult<List<Book>>> GetCataloges(
            [FromQuery]int count, 
            [FromQuery] int iter, 
            [FromQuery] int minPrice, 
            [FromQuery] int maxPrice, 
            [FromQuery] int genreId, 
            [FromQuery] string sort, 
            [FromQuery] string search = "")
        {
            var books = await _context.Books.Include(b => b.Gener).Where(b => b.Price <=maxPrice && b.Price >= minPrice).ToListAsync();
            if (genreId > 0)
            {
                books = books.Where(b => b.GenreId == genreId).ToList();
            }
            if(search.Length >= 3)
            {
                books = books.Where(b => b.BookName.ToLower().Contains(search.ToLower())).ToList();
            }

            switch(sort)
            {
                case "price_asc":
                    books = books.OrderBy(b => b.Price).Skip((iter - 1) * count).Take(count).OrderBy(b => b.Price).ToList();
                    break;
                case "price_desc":
                    books = books.OrderByDescending(b => b.Price).Skip((iter - 1) * count).Take(count).OrderByDescending(b => b.Price).ToList();
                    break;
                case "newest":
                    books = books.OrderByDescending(b => b.Id).Skip((iter - 1) * count).Take(count).OrderByDescending(b => b.Id).ToList();
                    break;
                case "oldest":
                    books = books.OrderBy(b => b.Id).Skip((iter - 1) * count).Take(count).OrderBy(b => b.Id).ToList();
                    break;
                default:
                    books = books.OrderByDescending(b => b.Id).Skip((iter - 1) * count).Take(count).OrderByDescending(b => b.Id).ToList();
                    break;
            }
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
        public async Task<ActionResult<GetBookWithComentDTO>> GetBook(int id)
        {
            var book = await _context.Books.Include(b => b.Gener).Include(b => b.Comments).ThenInclude(u => u.User).FirstOrDefaultAsync(b => b.Id == id);
            if (book is null) return NotFound("Book not found");

            var bookStruct = new GetBookWithComentDTO
            {
                Id = book.Id,
                Description = book.Description,
                Stock = book.Stock,
                Autor = book.Autor,
                BookName = book.BookName,
                Cover = book.Cover,
                Format = book.Format,
                Gener = book.Gener,
                GenreId = book.GenreId,
                Image = book.Image,
                Isbn = book.Isbn,
                Pagecount = book.Pagecount,
                Price = book.Price,
                Comments = book.Comments.Select(c => new CommentDTO
                {
                    Id = c.CommentId,
                    Message = c.Message,
                    UserName = c.User.Login
                }).ToList()
            };

            return Ok(bookStruct);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Book>> CreateBook([FromBody]CreateBookDTO createBookDTO)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity is null) return Unauthorized("User not found");
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
            if (identity is null) return Unauthorized("User not found");
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
            if (identity is null) return Unauthorized("User not found");
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
            if (identity is null) return Unauthorized("User not found");
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
            if (identity is null) return Unauthorized("User not found");
            var role = identity.Claims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value;
            if (role == "user") return Unauthorized("You are not admin");

            var book = await _context.Books.FindAsync(id);
            if (book is null) return NotFound("Book is not found");
            _context.Books.Remove(book);

            await _context.SaveChangesAsync();
            return Ok();
        }

        [Authorize]
        [HttpDelete("comment/{id}")]
        public async Task<ActionResult> RemoveComment(int id)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity is null) return Unauthorized("User not found");
            var role = identity.Claims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value;
            if (role == "user") return Unauthorized("You are not admin");

            var comment = await _context.Comment.FindAsync(id);
            if (comment is null) return NotFound("Comment is not found");

            _context.Comment.Remove(comment);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [Authorize]
        [HttpPost("comment/")]
        public async Task<ActionResult<CommentDTO>> CreateComment(CreateCommentDTO request)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity is null) return Unauthorized("User not found");
            var UserId = Convert.ToInt32(identity.Claims.FirstOrDefault(o => o.Type == ClaimTypes.Sid)?.Value);
            var user = await _context.Users.Include(u => u.Addresses).FirstOrDefaultAsync(u => u.Id == UserId);
            if (user is null) return Unauthorized("User not found");

            var comment = new Comment 
            { 
                BookId = request.BookId, 
                Message = request.Message, 
                UserId = UserId 
            };

            _context.Comment.Add(comment);
            await _context.SaveChangesAsync();
            var commentDTO = new CommentDTO { Id = comment.CommentId, Message = comment.Message, UserName = comment.User.Login };
            return Ok(commentDTO);
        }
    }
}
