using Microsoft.AspNetCore.DataProtection;
using ShopASP.Models;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace ShopASP.Utils
{
    public class Helpers
    {
        public static string GenSalt()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            const int lenght = 10;

            var bytes = new char[lenght];
            for (int i = 0; i < lenght; i++)
            {
                bytes[i] = chars[RandomNumberGenerator.GetInt32(chars.Length)];
            }

            return new string(bytes);
        }

        public static string Serialize<T>(IDataProtector protector, T obj)
        {
            string json = JsonSerializer.Serialize(obj);
            byte[] bytes = protector.Protect(Encoding.UTF8.GetBytes(json));
            return Convert.ToBase64String(bytes);
        }

        public static T? Deserialize<T>(IDataProtector protector, string val)
        {
            byte[] bytes = protector.Unprotect(Convert.FromBase64String(val));
            string json = Encoding.UTF8.GetString(bytes);
            return JsonSerializer.Deserialize<T>(json); ;
        }

        public static string PasswordToHash(string password, string salt)
        {
            if (password == null) throw new Exception("Password is null.");
            if (salt == null) throw new Exception("Salt is null.");

            byte[] hashedBytes = SHA256.HashData(Encoding.UTF8.GetBytes(password + salt));

            return Convert.ToHexString(hashedBytes);
        }

        public static User? CheckGetUser(ShopContext ctx, string userName, string password)
        {
            if (userName == null) throw new Exception("User name is null.");
            if (password == null) throw new Exception("Password is null.");

            var user = ctx.Users.FirstOrDefault(item => item.Username == userName);

            if (user == null) return null;

            string hashed = PasswordToHash(password, user.Salt);

            if (hashed != user.PasswordHash) return null;

            return user;
        }
    }
}
