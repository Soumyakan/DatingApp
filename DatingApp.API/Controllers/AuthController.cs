using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Model;
using DatingApp.API.DTO;
namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepsitoty _repo;
        public AuthController(IAuthRepsitoty repo){
            _repo = repo;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO registerDTO){
                registerDTO.UserName = registerDTO.UserName.ToLower();
                if(await _repo.UserExists(registerDTO.UserName)) return BadRequest("User Already exists");
                Users users= new Users();
                users.Name = registerDTO.UserName;
                var createdUser = await _repo.Register(users,registerDTO.password);
                return StatusCode(201);
        }
    }
}