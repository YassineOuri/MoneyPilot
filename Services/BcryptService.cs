using BCrypt.Net;

namespace MoneyPilot.Services
{
    public class BcryptService
    {
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.EnhancedHashPassword(password, workFactor : 12);
        } 

        public bool ValidatePassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.EnhancedVerify(password, hashedPassword);
        }
    }
}
