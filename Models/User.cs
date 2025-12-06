using MoneyPilot.DTO;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MoneyPilot.Models
{
    public class User
    {
    
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        [JsonIgnore]
        public string Password { get; set; } = string.Empty;

        [InverseProperty("Owner")]
        [JsonIgnore]
        public ICollection<Account>? Accounts { get; set; } = [];


        [InverseProperty("Owner")]
        [JsonIgnore]
        public ICollection<Transaction>? Transactions { get; set; } = [];


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
