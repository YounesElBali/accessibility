using wdpr_project.Models;

public class UserChat
{
    public string UserId { get; set; }
    public int ChatId { get; set; }
    public User User { get; set; }
    public Chat Chat { get; set; }
}
