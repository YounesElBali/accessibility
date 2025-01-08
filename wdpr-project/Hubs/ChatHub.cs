using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using wdpr_project.Data;
using wdpr_project.Models;

public class ChatHub : Hub
{
     private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        

        public ChatHub(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

public async Task SendMessage(string senderUsesId, string receiverUserId, string messageContent)
{
    // Get the sender user by username
    var senderUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == senderUsesId);
    var receiverUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == receiverUserId);
     
    if (senderUser != null && receiverUser != null)
    {
        Console.WriteLine($"Sender ID: {senderUser.Id}");
        Console.WriteLine($"Receiver ID: {receiverUser.Id}");

        // Get the chat room ID for the two users (create if doesn't exist)
        int chatRoomId = await CreateOrGetChatRoomIdAsync(receiverUser.Id,senderUser.Id);
        Console.WriteLine($"Chat Room ID: {chatRoomId}");

          // Ensure that the sender is part of the chat group
        await JoinGroup(new ChatUser { ChatId = chatRoomId });

        // Save the message to the chat room
        await SaveMessageAsync(chatRoomId, receiverUser.Id, messageContent);
        string formattedDateTime = DateTime.Now.ToString("HH,mm");
        
        // Broadcast the message to clients in the chat room
        await Clients.Group(chatRoomId.ToString()).SendAsync("newMessage", messageContent, formattedDateTime);

        Console.WriteLine($"Sent message from {senderUser.Id} to {receiverUser.Id}: {messageContent}");
    }
    else
    {
        Console.WriteLine("Sender or receiver not found.");
    }
}
 public async Task JoinGroup(ChatUser userConnection)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, userConnection.ChatId.ToString());
    }

private async Task<int> CreateOrGetChatRoomIdAsync(string user1Id, string user2Id)
{
    // Check if a chat room already exists for the two users
    var existingChatRoom = await _context.Chats
        .Where(c => c.UserChats.Any(u => u.UserId == user1Id) && c.UserChats.Any(u => u.UserId == user2Id))
        .FirstOrDefaultAsync();

    if (existingChatRoom != null)
    {
        Console.WriteLine("create room");
        return existingChatRoom.Id;
    }

    // If no chat room exists, create a new one
    return await CreateChatRoomAsync(user1Id, user2Id);
}
private async Task<int> CreateChatRoomAsync(string user1Id, string user2Id)
{
    // Check if a chat room already exists for the two users
    var existingChatRoom = await _context.Chats
        .AsNoTracking()  // Use AsNoTracking to prevent entity tracking
        .Where(c => c.UserChats.Any(u => u.UserId == user1Id) && c.UserChats.Any(u => u.UserId == user2Id))
        .FirstOrDefaultAsync();

    if (existingChatRoom != null)
    {
        return existingChatRoom.Id;
    }

    // If no chat room exists, create a new one
    var chatRoom = new Chat
    {
        UserChats = new List<UserChat>
            {
                new UserChat { User = user1Id },
                new UserChat { User = user2Id }
            },
            Messages = new List<Message>(),
    };

    _context.Chats.Add(chatRoom);
    await _context.SaveChangesAsync();

    return chatRoom.Id;
}


private async Task SaveMessageAsync(int chatRoomId, string senderUser, string messageContent)
{
    var chatRoom = await _context.Chats
        .Include(c => c.UserChats)
        .Include(c => c.Messages)
        .FirstOrDefaultAsync(c => c.Id == chatRoomId);
    var username = await _context.Users.FirstOrDefaultAsync(u => u.Id == senderUser);
        Console.WriteLine("current user username: "+ senderUser);
    if (chatRoom != null)
    {
         var newMessage = new Message
        {
            message = Regex.Replace(messageContent, @"<.*?>", string.Empty),
            UserId = senderUser,
            ChatId = chatRoomId,
            Username = username.UserName,
            Date = DateTime.Now
        };

        chatRoom.Messages.Add(newMessage);
        await _context.SaveChangesAsync();
    }
}
}