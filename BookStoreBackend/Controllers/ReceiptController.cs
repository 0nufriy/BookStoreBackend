using BookStoreBackend.Data;
using BookStoreBackend.Entities;
using BookStoreBackend.Jwt;
using BookStoreBackend.Migrations;
using BookStoreBackend.Models.AuthController;
using BookStoreBackend.Models.BookController;
using BookStoreBackend.Models.Enum;
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
        public async Task<ActionResult<List<ReceiptDTO>>> GetReceipts()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity is null) return Unauthorized("User not found");
            var role = identity.Claims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value;
            var id = Convert.ToInt32(identity.Claims.FirstOrDefault(o => o.Type == ClaimTypes.Sid)?.Value);
            var receipt = new List<ReceiptDTO>();
            if (role == "user")
            {
                receipt = await _context.Receipt
                    .Include(r => r.Books)
                    .ThenInclude(br => br.Book)
                    .Select(r => new ReceiptDTO { Id = r.Id, Address = r.Address, Books = r.Books, Status = r.Status, UserId = r.UserId })
                    .Where(r => r.UserId == id).ToListAsync();
            }
            else
            {
                receipt = await _context.Receipt
                    .Include(r => r.User)
                    .Include(r => r.Books)
                    .ThenInclude(br => br.Book)
                    .Select(r => new ReceiptDTO
                    {
                        Id = r.Id,
                        Address = r.Address,
                        Books = r.Books,
                        Status = r.Status,
                        UserId = r.UserId,
                        User = new UserDataDTO { Email = r.User.Email, Id = r.User.Id, Login = r.User.Login, Name = r.User.Name, Phone = r.User.Phone, Role = r.User.Role }
                    })
                    .ToListAsync();
            }


            return Ok(receipt);

        }
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<ReceiptDTO>> GetReceipt(int id)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity is null) return Unauthorized("User not found");
            var role = identity.Claims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value;
            var UserId = Convert.ToInt32(identity.Claims.FirstOrDefault(o => o.Type == ClaimTypes.Sid)?.Value);

            var receipt = await _context.Receipt.Include(b => b.Books).Include(b => b.User).FirstOrDefaultAsync(r => r.Id == id);
            if (receipt is null || (role == "user" && UserId != receipt.UserId)) return NotFound("Receipt not found");

            var receiptDTO = new ReceiptDTO
            {
                Id = receipt.Id,
                Address = receipt.Address,
                Books = receipt.Books,
                Status = receipt.Status,
                UserId = receipt.UserId,
                User = new UserDataDTO { Email = receipt.User.Email, Id = receipt.User.Id, Login = receipt.User.Login, Name = receipt.User.Name, Phone = receipt.User.Phone, Role = receipt.User.Role }
            };

            return Ok(receiptDTO);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> CreateReceipt([FromBody] CreateReceiptDTO createReceiptDTO)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity is null) return Unauthorized("User not found");
            var UserId = Convert.ToInt32(identity.Claims.FirstOrDefault(o => o.Type == ClaimTypes.Sid)?.Value);

            var user = await _context.Users.Include(u => u.Addresses).FirstOrDefaultAsync(u => u.Id == UserId);
            if(user is null) return Unauthorized("User not found");
            if(!user.Addresses.Where(a => a.Id == createReceiptDTO.AddressId).Any()) return NotFound("Address not found");
            var address = await _context.Addresses.FindAsync(createReceiptDTO.AddressId);
            if(address is null) return BadRequest("Can not find address " + createReceiptDTO.AddressId);

            var BooksReceipt = new List<BookReceipt>();

            for (int i = 0; i < createReceiptDTO.Books.Count; i++) {
                var book = await _context.Books.FindAsync(createReceiptDTO.Books[i].BookId);
                if (book is null) return BadRequest("Can not find book " + createReceiptDTO.Books[i].BookId);
                if (book.Stock < createReceiptDTO.Books[i].Count) return BadRequest("No book in stock");
                book.Stock = book.Stock - createReceiptDTO.Books[i].Count;
                BooksReceipt.Add(new BookReceipt() { BookId = createReceiptDTO.Books[i].BookId, Count = createReceiptDTO.Books[i].Count });
            }

            var newReceipt = new Receipt()
            {
               UserId = UserId,
               Books = BooksReceipt,
               Address = address.Adress,
               Status = StatusEnum.Pending
            };
            _context.Receipt.Add(newReceipt);  
            await _context.SaveChangesAsync();
            return Ok();
        }

        [Authorize]
        [HttpPut]
        public async Task<ActionResult<Receipt>> ChangeStatus([FromQuery] int receiptId, [FromQuery] StatusEnum status)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity is null) return Unauthorized("User not found");


            var role = identity.Claims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value;
            if(role == "user") return Unauthorized("User not admin");

            var UserId = Convert.ToInt32(identity.Claims.FirstOrDefault(o => o.Type == ClaimTypes.Sid)?.Value);
            var user = await _context.Users.Include(u => u.Addresses).FirstOrDefaultAsync(u => u.Id == UserId);
            if (user is null) return Unauthorized("User not found");

            var receipt = await _context.Receipt.Include(r => r.Books).Include(r => r.User).FirstOrDefaultAsync(r => r.Id == receiptId);
            if (receipt is null) return NotFound("Receipt not found");

            receipt.Status = status;
            await _context.SaveChangesAsync();


            await _context.Entry(receipt)
                    .Collection(re => re.Books)
                    .LoadAsync();

            var receiptDTO = new ReceiptDTO
            {
                Id = receipt.Id,
                Address = receipt.Address,
                Books = receipt.Books,
                Status = receipt.Status,
                UserId = receipt.UserId,
                User = new UserDataDTO { Email = receipt.User.Email, Id = receipt.User.Id, Login = receipt.User.Login, Name = receipt.User.Name, Phone = receipt.User.Phone, Role = receipt.User.Role }
            };

            return Ok(receiptDTO);
        }
    }
}
