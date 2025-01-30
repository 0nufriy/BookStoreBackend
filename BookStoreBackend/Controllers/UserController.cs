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
        public async Task<ActionResult<List<UserDataDTO>>> GetAllUsers()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity is null) return Unauthorized("User not found");
            var role = identity.Claims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value;
            if (role == "user") return Unauthorized("You are not admin");


            var users = await _context.Users.Include(u => u.Addresses)
                .Select(u => new UserDataDTO { 
                    Email = u.Email,
                    Phone = u.Phone,
                    Login = u.Login,
                    Role = u.Role,
                    Addresses = u.Addresses,
                    Id = u.Id,
                    Name = u.Name,
                })
                .ToListAsync();

            return Ok(users);
        }

        [HttpGet]
        public async Task<ActionResult<UserDataDTO>> GetUserById()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if(identity is null) return Unauthorized("User not found");
            var id = Convert.ToInt32(identity.Claims.FirstOrDefault(o => o.Type == ClaimTypes.Sid)?.Value);

            var user = await _context.Users.FindAsync(id);
            if (user is null) return Unauthorized("User not found");
            await _context.Entry(user)
                    .Collection(u => u.Addresses)
                    .LoadAsync();

            var userDTO = new UserDataDTO
            {
                Email = user.Email,
                Phone = user.Phone,
                Login = user.Login,
                Role = user.Role,
                Addresses = user.Addresses,
                Id = user.Id,
                Name = user.Name,
            };

            return Ok(userDTO);
        }

        [HttpPut]
        public async Task<ActionResult<UserDataDTO>> UpdateUser([FromBody]UpdateUserDTO user)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity is null) return Unauthorized("User not found");
            var id = Convert.ToInt32(identity.Claims.FirstOrDefault(o => o.Type == ClaimTypes.Sid)?.Value);

            var userDb = await _context.Users.FindAsync(id);
            if (userDb is null) return Unauthorized("User not found");

            await _context.Entry(userDb)
                   .Collection(u => u.Addresses)
                   .LoadAsync();

            userDb.Phone = user.Phone;
            userDb.Login = user.Login;
            userDb.Email = user.Email;
            userDb.Name = user.Name;

            await _context.SaveChangesAsync();

            var userDTO = new UserDataDTO
            {
                Email = userDb.Email,
                Phone = userDb.Phone,
                Login = userDb.Login,
                Role = userDb.Role,
                Addresses = userDb.Addresses,
                Id = userDb.Id,
                Name = userDb.Name,
            };

            return Ok(userDTO);
        }
        [HttpDelete]
        public async Task<ActionResult> DeleteUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity is null) return Unauthorized("User not found");
            var id = Convert.ToInt32(identity.Claims.FirstOrDefault(o => o.Type == ClaimTypes.Sid)?.Value);

            var userDb = await _context.Users.FindAsync(id);
            if (userDb is null) return Unauthorized("User not found");
            _context.Users.Remove(userDb);


            await _context.SaveChangesAsync();
            return Ok();
        }
        [HttpPost("address")]
        public async Task<ActionResult<UserDataDTO>> AddAddress(AddAddressDTO address)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity is null) return Unauthorized("User not found");
            var id = Convert.ToInt32(identity.Claims.FirstOrDefault(o => o.Type == ClaimTypes.Sid)?.Value);

            var userDb = await _context.Users.FindAsync(id);
            if (userDb is null) return Unauthorized("User not found");

            await _context.Entry(userDb)
                   .Collection(u => u.Addresses)
                   .LoadAsync();

            var addressToAdd = new UserAddress
            {
                UserId = id,
                Adress = address.City + ", " + address.Street + " " + address.House + ", " + address.PostalCode,
                AdressName = address.AdressName
            };
            userDb.Addresses.Add(addressToAdd);

            await _context.SaveChangesAsync();

            var userDTO = new UserDataDTO
            {
                Email = userDb.Email,
                Phone = userDb.Phone,
                Login = userDb.Login,
                Role = userDb.Role,
                Addresses = userDb.Addresses,
                Id = userDb.Id,
                Name = userDb.Name,
            };

            return Ok(userDTO);
        }

        [HttpDelete("address/{addressId}")]
        public async Task<ActionResult<UserDataDTO>> DelleteAddress(int addressId)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity is null) return Unauthorized("User not found");
            var id = Convert.ToInt32(identity.Claims.FirstOrDefault(o => o.Type == ClaimTypes.Sid)?.Value);

            var userDb = await _context.Users.FindAsync(id);
            if (userDb is null) return Unauthorized("User not found");

            var address = await _context.Addresses.FindAsync(addressId);

            if (address is null || address.UserId != userDb.Id) return NotFound("Address not found");
            
            _context.Addresses.Remove(address);


            await _context.SaveChangesAsync();

            await _context.Entry(userDb)
                   .Collection(u => u.Addresses)
                   .LoadAsync();

            var userDTO = new UserDataDTO
            {
                Email = userDb.Email,
                Phone = userDb.Phone,
                Login = userDb.Login,
                Role = userDb.Role,
                Addresses = userDb.Addresses,
                Id = userDb.Id,
                Name = userDb.Name,
            };

            return Ok(userDTO);
        }
    }
}
