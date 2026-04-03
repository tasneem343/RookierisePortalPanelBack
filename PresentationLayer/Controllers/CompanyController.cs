using BusinessLayer.DTOS;
using BusinessLayer.Interfaces;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PresentationLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyManager _companyManager;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public CompanyController(ICompanyManager companyManager, IEmailService emailService, IConfiguration configuration)
        {
            _companyManager = companyManager;
            _emailService = emailService;
            _configuration = configuration;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromBody] CompanySignUpDto dto)
        {
            var company = new Company
            {
                CompanyNameArabic = dto.CompanyNameArabic,
                CompanyNameEnglish = dto.CompanyNameEnglish,
                WebsiteUrl = dto.WebsiteUrl,
                PhoneNumber = dto.PhoneNumber,
                Email = dto.Email,
                CompanyLogo = dto.CompanyLogo
            };

            await _companyManager.CreateCompanyAsync(company);

            var body = $@"
                <p>Hi {dto.Email},</p>
                <p>Please use the link below to set your password:</p>
                <a href='http://localhost:4200/setpassword?email={dto.Email}'>Set Password</a>
            ";

            await _emailService.SendEmailAsync(dto.Email, "Set Your Password", body);

            return Ok(new { message = "Company registered successfully, please check your email." });
        }



        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthDto dto)
        {

            var company = await _companyManager.GetByEmailAsync(dto.Email);
            if (company == null)
                return NotFound(new { message = "Email not found." });

            if (string.IsNullOrEmpty(company.PasswordHash))
                return BadRequest(new { message = "Please set your password first." });

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, company.PasswordHash))
                return Unauthorized(new { message = "Invalid password." });

            var token = GenerateJwtToken(company.Email);
            return Ok(new { message = "Login successful.", token });
        }

        private string GenerateJwtToken(string email)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]!));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
        new Claim(ClaimTypes.Email, email)
    };

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(
                    int.Parse(_configuration["JwtSettings:ExpiryDays"]!)),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        [HttpPost("set-password")]

        public async Task<IActionResult> SetPassword([FromBody] AuthDto dto)
        {
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            await _companyManager.SetPasswordAsync(dto.Email, passwordHash);

            return Ok(new { message = "Password set successfully." });
        }
    }
}

