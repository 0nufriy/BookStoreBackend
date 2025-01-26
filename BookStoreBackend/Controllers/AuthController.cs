using BookStoreBackend.Data;
using BookStoreBackend.Entities;
using BookStoreBackend.Jwt;
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
    public class AuthController : ControllerBase
    {
        private readonly DataContext _context;

        private readonly IConfiguration _configuration;
        public AuthController(DataContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserWithTokenDTO>> Login(ToLoginDTO login)
        {

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == login.Login && u.Password == login.Pasword);
            if (user is null) return NotFound("User not found");

            await _context.Entry(user)
                    .Collection(u => u.Addresses)
                    .LoadAsync();

            var userWithToken = new UserWithTokenDTO
            {
                Email = user.Email,
                Login = user.Login,
                Name = user.Name,
                Phone = user.Phone,
                Role = user.Role,
                Token = JwtBearer.CreateToken(_configuration, user),
                Id = user.Id
            };

            return Ok(userWithToken);
        }

        [HttpPost("registr")]
        public async Task<ActionResult<UserWithTokenDTO>> CreateUser([FromBody] CreateUserDTO user)
        {
            var usercheck = await _context.Users.FirstOrDefaultAsync(u => u.Login == user.Login || u.Email == user.Email || u.Phone == user.Phone);
            if (usercheck is not null) return NotFound("User allready exist");
            var userToAdd = new User
            {
                Login = user.Login,
                Email = user.Email,
                Name = user.Name,
                Password = user.Password,
                Phone = user.Phone,
                Role = "user"
            };
            _context.Users.Add(userToAdd);

            await _context.SaveChangesAsync();

            var userAdded = await _context.Users.FirstAsync(u => u.Login == user.Login && u.Password == user.Password);
            if (user is null) return NotFound("Can`t create");

            var userWithToken = new UserWithTokenDTO
            {
                Email = userAdded.Email,
                Login = userAdded.Login,
                Name = userAdded.Name,
                Phone = userAdded.Phone,
                Role = userAdded.Role,
                Token = JwtBearer.CreateToken(_configuration, userAdded),
                Id = userAdded.Id
            };

            return Ok(userWithToken);
        }
        [Authorize]
        [HttpPost("registrAdmin")]
        public async Task<ActionResult<UserWithTokenDTO>> CreateAdmin([FromBody] CreateUserDTO user)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity is null) return Unauthorized("User not found");
            var role = identity.Claims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value;
            if (role == "user") return Unauthorized("You are not admin");

            var usercheck = await _context.Users.FirstOrDefaultAsync(u => u.Login == user.Login || u.Email == user.Email || u.Phone == user.Phone);
            if (usercheck is not null) return NotFound("User allready exist");

            var userToAdd = new User
            {
                Login = user.Login,
                Email = user.Email,
                Name = user.Name,
                Password = user.Password,
                Phone = user.Phone,
                Role = "admin"
            };
            _context.Users.Add(userToAdd);

            await _context.SaveChangesAsync();

            var userAdded = await _context.Users.FirstAsync(u => u.Login == user.Login && u.Password == user.Password);
            if (user is null) return NotFound("Can`t create");

            var userWithToken = new UserWithTokenDTO
            {
                Email = userAdded.Email,
                Login = userAdded.Login,
                Name = userAdded.Name,
                Phone = userAdded.Phone,
                Role = userAdded.Role,
                Token = JwtBearer.CreateToken(_configuration, userAdded),
                Id = userAdded.Id
            };

            return Ok(userWithToken);
        }
    }
}
