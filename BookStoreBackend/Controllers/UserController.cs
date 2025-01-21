using BookStoreBackend.Data;
using BookStoreBackend.Entities;
using BookStoreBackend.Models.AuthController;
using BookStoreBackend.Models.UserController;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BookStoreBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly DataContext _context;

        public UserController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<User>>> GetAllUsers()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity is null) return NotFound("User not found");
            var role = identity.Claims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value;
            if (role == "user") return Unauthorized("You are not admin");


            var users = await _context.Users.Include(u => u.Addresses).ToListAsync();

            return Ok(users);
        }

        [HttpGet]
        public async Task<ActionResult<User>> GetUserById()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if(identity is null) return NotFound("User not found");
            var id = Convert.ToInt32(identity.Claims.FirstOrDefault(o => o.Type == ClaimTypes.Sid)?.Value);

            var user = await _context.Users.FindAsync(id);
            if (user is null) return NotFound("User not found");
            await _context.Entry(user)
                    .Collection(u => u.Addresses)
                    .LoadAsync();
            return Ok(user);
        }
        [HttpPut]
        public async Task<ActionResult<User>> UpdateUser([FromBody]UpdateUserDTO user)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity is null) return NotFound("User not found");
            var id = Convert.ToInt32(identity.Claims.FirstOrDefault(o => o.Type == ClaimTypes.Sid)?.Value);

            var userDb = await _context.Users.FindAsync(id);
            if (userDb is null) return NotFound("User not found");
            userDb.Phone = user.Phone;
            userDb.Login = user.Login;
            userDb.Email = user.Email;
            userDb.Name = user.Name;

            await _context.SaveChangesAsync();
            return Ok(userDb);
        }
        [HttpDelete]
        public async Task<ActionResult> DeleteUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity is null) return NotFound("User not found");
            var id = Convert.ToInt32(identity.Claims.FirstOrDefault(o => o.Type == ClaimTypes.Sid)?.Value);

            var userDb = await _context.Users.FindAsync(id);
            if (userDb is null) return NotFound("User not found");
            _context.Users.Remove(userDb);


            await _context.SaveChangesAsync();
            return Ok();
        }
        [HttpPost("address")]
        public async Task<ActionResult<User>> AddAddress(AddAddressDTO address)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity is null) return NotFound("User not found");
            var id = Convert.ToInt32(identity.Claims.FirstOrDefault(o => o.Type == ClaimTypes.Sid)?.Value);

            var userDb = await _context.Users.FindAsync(id);
            if (userDb is null) return NotFound("User not found");
            var addressToAdd = new UserAddress
            {
                UserId = id,
                Adress = address.Adress,
                AdressName = address.AdressName
            };
            userDb.Addresses.Add(addressToAdd);

            await _context.SaveChangesAsync();
            return Ok(userDb);
        }
    }
}
