using System.Threading.Tasks;
using DatingApp.API.Model;
using System.Security.Cryptography;
using DatingApp.API.Controllers;
using Microsoft.EntityFrameworkCore;
using DatingApp.API.Data;
namespace DatingApp.API.Data
{
    public class AuthRepositoty : IAuthRepsitoty
    {
        public DataContext _context;
        public AuthRepositoty(DataContext dataContext){
            _context = dataContext;
        }
        public async Task<Users> Login(string userName, string password)
        {
            var user = await  _context.Users.FirstOrDefaultAsync( x => x.Name == userName);
            if(user == null){
                return null;
            }

            if(!VerifyPasswordHash(password,user.PasswordHash,user.PassworSalt)){
                return null;
            }
        return user;

        }

        public bool VerifyPasswordHash(string password, byte[] passHash ,byte[] passSalt){

            using(var hmac = new System.Security.Cryptography.HMACSHA512(passSalt)){
                var computeHash =hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for(int i = 0 ; i<computeHash.Length ; i++){
                    if (computeHash[i] != passHash[i]){
                        return false;
                    }
                    
                }
                
            }
            return true;

        }

        public async Task<Users> Register(Users users, string password)
        {
            byte[] passHash;
            byte[] passSalt;
            createPasswordHash(password, out passHash, out passSalt);
            users.PasswordHash=passHash;
            users.PassworSalt=passSalt;

            await _context.Users.AddAsync(users);
            await _context.SaveChangesAsync();

            return users;
            
        }

        void createPasswordHash(string password , out byte[] passHash,  out byte[] passSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512()){
                passSalt = hmac.Key;
                passHash =hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<bool> UserExists(string userName)
        {
            if(await _context.Users.AnyAsync(x => x.Name == userName)){
                return true;
            }
            return false;
        }
    }
} 