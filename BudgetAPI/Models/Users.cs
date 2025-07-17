namespace BudgetAPI.Models
{
    public class Users
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string EmailAddress { get; set; }
        public string Role { get; set; }
        public string Surname { get; set; }
        public string GivenName { get; set; }
    }

    public class UserLogin
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
