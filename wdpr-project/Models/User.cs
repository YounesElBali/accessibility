using Microsoft.AspNetCore.Identity;

namespace wdpr_project.Models
{
    public class User : IdentityUser
    {
        public string Password { get; set; } // TODO: change to something more secure
        public virtual ICollection<UserChat>? UserChats { get; set; }
        public User() { }

        public User(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }

        public static implicit operator User(string v)
        {
            throw new NotImplementedException();
        }
    }
}
