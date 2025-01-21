using BookStoreBackend.Data;
using BookStoreBackend.Entities;
using BookStoreBackend.Jwt;
using BookStoreBackend.Migrations;
using BookStoreBackend.Models.AuthController;
using BookStoreBackend.Models.BookController;
using BookStoreBackend.Models.ReceiptController;
using BookStoreBackend.Models.UserController;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Server;
using System.Security.Claims;
using static System.Net.Mime.MediaTypeNames;
using static System.Reflection.Metadata.BlobBuilder;

namespace BookStoreBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceiptController : ControllerBase
    {
        private readonly DataContext _context;

        public ReceiptController(DataContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<List<Receipt>>> GetBooks()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity is null) return NotFound("User not found");
            var role = identity.Claims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value;
            var id = Convert.ToInt32(identity.Claims.FirstOrDefault(o => o.Type == ClaimTypes.Sid)?.Value);
            if (role == "user")
            {
                var receipt = await _context.Receipt.Include(r => r.User).Include(r => r.Books).ThenInclude(br => br.Book).Where(r => r.UserId == id).ToListAsync();
                return Ok(receipt);
            }
            else
            {
                var receipt = await _context.Receipt.Include(r => r.User).Include(r => r.Books).ThenInclude(br => br.Book).ToListAsync();
                return Ok(receipt);
            }

            
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Receipt>> GetReceipt(int id)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity is null) return NotFound("User not found");
            var role = identity.Claims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value;
            var UserId = Convert.ToInt32(identity.Claims.FirstOrDefault(o => o.Type == ClaimTypes.Sid)?.Value);

            var receipt = await _context.Receipt.Include(b => b.Books).FirstOrDefaultAsync(r => r.Id == id);
            if (receipt is null || (role == "user" && UserId != receipt.UserId)) return NotFound("Book not found");
            return Ok(receipt);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Book>> CreateReceipt([FromBody] CreateReceiptDTO createReceiptDTO)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity is null) return NotFound("User not found");
            var UserId = Convert.ToInt32(identity.Claims.FirstOrDefault(o => o.Type == ClaimTypes.Sid)?.Value);

            var user = await _context.Users.Include(u => u.Addresses).FirstOrDefaultAsync(u => u.Id == UserId);
            if(user is null) return NotFound("User not found");
            if(!user.Addresses.Where(a => a.Id == createReceiptDTO.AddressId).Any()) return NotFound("Address not found");
            var address = await _context.Addresses.FindAsync(createReceiptDTO.AddressId);

            var BooksReceipt = new List<BookReceipt>();

            for (int i = 0; i < createReceiptDTO.Books.Count; i++)
            {
                BooksReceipt.Add(new BookReceipt() { BookId = createReceiptDTO.Books[i].BookId, Count = createReceiptDTO.Books[i].Count });
            }

            var newReceipt = new Receipt()
            {
               UserId = UserId,
               Books = BooksReceipt,
               Address = address.Adress
            };
            _context.Receipt.Add(newReceipt);  
            await _context.SaveChangesAsync();
            return Ok(newReceipt);
        }
    }
}
