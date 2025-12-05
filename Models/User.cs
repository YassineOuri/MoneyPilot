using MoneyPilot.DTO;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyPilot.Models
{
    public class User
    {
    
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        [InverseProperty("Owner")]
        
        public ICollection<Account> accounts { get; set; } = new List<Account>();


        public User()
        {

        }

        public User(string firstName, string lastName, string email, string password)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
        }
    }
}
