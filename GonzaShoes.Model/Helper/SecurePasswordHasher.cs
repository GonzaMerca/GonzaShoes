using System.Security.Cryptography;
using System.Text;

namespace GonzaShoes.Model.Helper
{
    public class SecurePasswordHasher
    {
        public string HashPassword(string password)
        {
            using (var sha256 = SHA1.Create())
            {
                string hexString = Convert.ToHexString(Encoding.UTF8.GetBytes(password));
                byte[] hashValue = sha256.ComputeHash(Encoding.UTF8.GetBytes(hexString));
                return Convert.ToHexString(hashValue);
            }
        }

        public bool Verify(string enteredPassword, string storedHashedPassword)
        {
            return storedHashedPassword == HashPassword(enteredPassword);
        }
    }
}
