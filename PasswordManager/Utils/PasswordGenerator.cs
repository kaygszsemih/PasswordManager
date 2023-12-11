using System.Security.Cryptography;
using System.Text;

namespace PasswordManager.Utils
{
    public static class PasswordGenerator
    {
        public static string GeneratePassword()
        {
            const string characters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*";

            StringBuilder characterSet = new StringBuilder(characters);

            using var rng = new RNGCryptoServiceProvider();
            var byteArray = new byte[1];
            StringBuilder password = new StringBuilder();

            for (int i = 0; i < 12; i++)
            {
                rng.GetBytes(byteArray);
                var randomChar = characterSet[byteArray[0] % characterSet.Length];
                password.Append(randomChar);
            }

            return password.ToString();
        }
    }
}
