using Api.Api.Application;
using Api.Api.Application.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Api.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private DataContext _context;
        private IConfiguration _configuration;

        public AuthController(IConfiguration configuration, DataContext dataContext)
        {
            _context = dataContext;
            _configuration = configuration; 
        }

        [HttpGet]
        public async Task<ActionResult<UserDto>> Get()
        {
            return Ok(await _context.Users.ToListAsync());
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserDto request)
        {
            var userDb = await _context.Users.FirstOrDefaultAsync(user => user.Username == request.Username);

            if(userDb == null)
            {
                Console.WriteLine(request);
                var newUser = new User();
                CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
                
                newUser.Username = request.Username;
                newUser.PasswordHash = passwordHash;
                newUser.PasswordSalt = passwordSalt; 
                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();

                return Ok("User created successfully");
            }
            else
            {
                return BadRequest("User already exists");
            }
            
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserDto user)
        {
            var userDb = await _context.Users.FirstOrDefaultAsync(u => u.Username == user.Username);

            if(userDb == null)
            {
                return NotFound("User not found");
            }
            if(!VerifyPassword(user.Password, userDb.PasswordHash, userDb.PasswordSalt))
            {
                return BadRequest("Username or password incorrect");
            }

            string token = CreateToken(userDb);

            return token;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

            }
        }

        private bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt)
        {
           
            using(var hmac = new HMACSHA512(passwordSalt))
            {
                var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                return computeHash.SequenceEqual(passwordHash);
            }
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
