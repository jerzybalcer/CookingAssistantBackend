using CookingAssistantBackend.Models;
using CookingAssistantBackend.Models.Database;
using CookingAssistantBackend.Utilis;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace CookingAssistantBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : CustomController
    {
        private readonly CookingAssistantContext _context;

        public UsersController(CookingAssistantContext context)
        {
            _context = context;
        }

        public static UserAccount account = new UserAccount();

        [HttpPost("register")]
        public async Task<IActionResult> Register(AuthenticateRequest request)
        {
            //CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            account.Email = request.Username;
            account.HashedPassword = request.Password;

            UserAccount user = new UserAccount(request.Username, request.Password);

            _context.UserAccounts.Add(user);
            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(AuthenticateRequest request)
        {
            bool exist = _context.UserAccounts
                .Any(r => r.Email == request.Username.ToLower() && r.HashedPassword == request.Password);

            if (!exist)
                return BadRequest("no user found");

            return Ok("cos dziala");
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using(var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

    }
}
