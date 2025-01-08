using wdpr_project.Models;

public class Chat{
    public int Id{get;set;}
    public ICollection<UserChat> UserChats { get; set; }
        
    public ICollection<Message> Messages { get; set; }
}