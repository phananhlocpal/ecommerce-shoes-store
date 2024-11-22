using backend.Dtos;
using backend.Entities;
using backend.Models;
using backend.Repositories.AuthRepo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenController : ControllerBase
    {
        private readonly IAuthRepo _authenRepo;
        private readonly IConfiguration _configuration;

        public AuthenController(IAuthRepo authenRepo, IConfiguration configuration)
        {
            _authenRepo = authenRepo;
            _configuration = configuration;
        }

        // POST: api/Authen/Login (Login User)
        [HttpPost("login-user")]
        public async Task<ActionResult> Login([FromBody] LoginRequestVM loginRequest)
        {
            var user = await _authenRepo.ValidateUserCredentialsAsync(loginRequest.Email, loginRequest.Password);
            if (user == null)
            {
                return Unauthorized("Invalid username or password.");
            }

            var token = GenerateJwtToken(user);
            SetAuthCookie(token);

            return Ok(new { Token = token, Id = user.Id });
        }

        [HttpPost("login-customer")]
        public async Task<ActionResult> LoginCustomer([FromBody] LoginRequestVM loginRequest)
        {
            var customer = await _authenRepo.ValidateCustomerCredentialsAsync(loginRequest.Email, loginRequest.Password);
            if (customer == null)
            {
                return Unauthorized("Invalid username or password.");
            }

            var token = GenerateJwtToken(customer);
            SetAuthCookie(token);

            return Ok(new { Token = token, Customer = customer });
        }

        // POST: api/Authen/Register(Register Customer)
        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] CustomerSignUpVM registerRequest)
        {
            var customer = await _authenRepo.RegisterCustomerAsync(registerRequest);
            if (customer == null)
            {
                return BadRequest("Registration failed.");
            }

            var token = GenerateJwtToken(customer);
            SetAuthCookie(token);

            return Ok(new { Token = token });
        }

        // POST: api/Authen/Logout (Logout User)
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("AuthCookie");
            return Ok("Logged out successfully.");
        }

        // POST: api/Authen/refresh-token
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            // Retrieve the existing token from the cookie
            var currentToken = Request.Cookies["AuthCookie"];
            if (string.IsNullOrEmpty(currentToken))
            {
                return Unauthorized("No token found.");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                // Validate and decode the current token to extract the claims
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false, // We don't want to validate the expiration yet
                    ClockSkew = TimeSpan.Zero // Remove clock skew allowance
                };

                var claimsPrincipal = tokenHandler.ValidateToken(currentToken, tokenValidationParameters, out var validatedToken);
                var roleClaim = claimsPrincipal.FindFirst("Role")?.Value;

                if (roleClaim == null)
                {
                    return Unauthorized("Invalid token.");
                }

                // Based on the role, fetch the correct user or customer
                object userOrCustomer;

                if (roleClaim == "User")
                {
                    var usernameClaim = claimsPrincipal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
                    userOrCustomer = await _authenRepo.GetUserByEmailAsync(usernameClaim);
                }
                else if (roleClaim == "Customer")
                {
                    var emailClaim = claimsPrincipal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
                    userOrCustomer = await _authenRepo.GetCustomerByEmailAsync(emailClaim);
                }
                else
                {
                    return Unauthorized("Invalid token role.");
                }

                // Regenerate a new token
                var newToken = GenerateJwtToken(userOrCustomer);
                SetAuthCookie(newToken);

                return Ok(new { Token = newToken });
            }
            catch (Exception ex)
            {
                return Unauthorized($"Invalid token: {ex.Message}");
            }
        }

        private string GenerateJwtToken(object userOrCustomer)
        {
            // Check the type of the user or customer and generate appropriate claims
            Claim[] claims;

            if (userOrCustomer is User user)
            {
                claims = new[]
                {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("Role", "User") 
                };
            }
            else if (userOrCustomer is CustomerReadDto customer)
            {
                claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, customer.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("Role", "Customer")
                };
            }
            else
            {
                throw new ArgumentException("Invalid object type");
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(20),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private void SetAuthCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = DateTime.Now.AddDays(1)
            };
            Response.Cookies.Append("AuthCookie", token, cookieOptions);
        }


    }
}
