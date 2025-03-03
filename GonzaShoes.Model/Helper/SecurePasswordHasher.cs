using System.Security.Cryptography;
using System.Text;

namespace GonzaShoes.Model.Helper
{
    public class SecurePasswordHasher
    {
        public static string HashPassword(string password)
        {
            // Paso 1: Convertimos la contraseña a un arreglo de bytes usando UTF-8.
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

            // Paso 2: Aplicamos SHA1 al arreglo de bytes.
            using (var sha1 = SHA1.Create())
            {
                byte[] hashValue = sha1.ComputeHash(passwordBytes);

                // Paso 3: Convertimos el hash en bytes a una cadena hexadecimal en minúsculas.
                return BitConverter.ToString(hashValue).Replace("-", "");
            }
        }

        public static bool Verify(string enteredPassword, string storedHashedPassword)
        {
            return storedHashedPassword == HashPassword(enteredPassword);
        }
    }
}