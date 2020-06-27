using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Model;
using DatingApp.API.DTO;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class AuthController : ControllerBase
    {
        public IAuthRepsitoty _repo;
        private readonly IConfiguration _config;
        public AuthController(IAuthRepsitoty repo, IConfiguration config)
        {
            _config = config;
            _repo = repo;
        }



        [HttpPost("register")]
        
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            registerDTO.UserName = registerDTO.UserName.ToLower();
            if (await _repo.UserExists(registerDTO.UserName)) return BadRequest("User Already exists");
            Users users = new Users();
            users.Name = registerDTO.UserName;
            var createdUser = await _repo.Register(users, registerDTO.password);
            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            var userFromRepo = await _repo.Login(loginDTO.UserName, loginDTO.password);

            if (userFromRepo == null) return Unauthorized();
            //create token 
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name, userFromRepo.Name)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings : Token").Value));
            var creds = new SigningCredentials(key,SecurityAlgorithms.HmacSha384Signature);
            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };
            
            var tokenHandler = new JwtSecurityTokenHandler();
            var Token = tokenHandler.CreateToken(tokenDescription);

            return Ok(new {
                Token = tokenHandler.WriteToken(Token)
            });

        }
    }
}