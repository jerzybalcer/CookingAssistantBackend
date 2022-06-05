using Backend.Services;
using CookingAssistantBackend.Models;
using CookingAssistantBackend.Models.Database;
using CookingAssistantBackend.Utilis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Cryptography;

namespace CookingAssistantBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : CustomController
    {
        private readonly CookingAssistantContext _context;
        private readonly ITokenService _tokenService;

        public UsersController(CookingAssistantContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public static UserAccount account = new UserAccount();

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(AuthenticateRequest request)
        {
            //CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            account.Email = request.Username;
            account.HashedPassword = request.Password;

            UserAccount user = new UserAccount(request.Username, request.Password);

            _context.UserAccounts.Add(user);
            await _context.SaveChangesAsync();
            return Ok(user);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(AuthenticateRequest request)
        {
            var user = await _context.UserAccounts.Include(u => u.User)
                .Where(r => r.Email == request.Username.ToLower() && r.HashedPassword == request.Password)
                .FirstOrDefaultAsync();

            if (user == null)
                return BadRequest("no user found");


            var token = _tokenService.GenerateToken(user);

            user.RefreshToken = _tokenService.GenerateRefreshToken();
            user.RefreshTokenExpiries = DateTime.Now.AddHours(2);

            await _context.SaveChangesAsync();
            return Ok(new Tuple<string, string>(token, user.RefreshToken));
        }

        [HttpPost("UpdateWorker")]
        public async Task<IActionResult> UpdateWorker(User user)
        {
            var oldUser = await _context.Users.FirstOrDefaultAsync(u => u.UserId == user.UserId);

            if (oldUser == null)
            {
                return NotFound("User not found");
            }

            var oldAccount = await _context.UserAccounts.FirstOrDefaultAsync(ua => ua.UserAccountId == user.UserAccountId);

            if (oldAccount == null)
            {
                return NotFound("Account not found");
            }

            oldUser = user;

            await _context.SaveChangesAsync();

            return Ok();
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
