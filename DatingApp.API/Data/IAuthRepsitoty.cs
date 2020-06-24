using System.Threading.Tasks;
using DatingApp.API.Model;
namespace DatingApp.API.Data
{
    public interface IAuthRepsitoty
    {
        // Three methods inside 1. For registering 2. For login 3. To check if user exists
          Task<Users> Register(Users users, string password);
          Task<Users> Login(string userName ,string password);
          Task<bool> UserExists(string userName);

    }
}