using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.BusinessLogic.Hashing
{
    public static class PasswordHasher
    {
        public static string GenerateSHA256Hash(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Convert the input string to a byte array and compute the hash.
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Convert the byte array to a string.
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public static bool VerifySHA256Hash(string inputPassword, string storedHash)
        {
            // Hash the input.
            var hashOfInput = GenerateSHA256Hash(inputPassword);

            return string.Equals(hashOfInput, storedHash, StringComparison.CurrentCultureIgnoreCase);
        }
    }
}
