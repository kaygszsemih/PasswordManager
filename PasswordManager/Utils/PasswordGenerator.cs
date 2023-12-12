using System.Security.Cryptography;
using System.Text;

namespace PasswordManager.Utils
{
    public static class PasswordGenerator
    {
        public static string GeneratePassword()
        {
            const string characters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*";
            StringBuilder password = new();
            byte[] randomNumber = new byte[1];

            for (int i = 0; i < 12; i++)
            {
                RandomNumberGenerator.Fill(randomNumber);
                var randomChar = characters[randomNumber[0] % characters.Length];
                password.Append(randomChar);
            }

            return password.ToString();
        }
    }
}
