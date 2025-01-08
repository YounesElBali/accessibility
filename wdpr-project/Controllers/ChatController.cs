using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using wdpr_project.Models;
using wdpr_project.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using wdpr_project.DTOs;


[ApiController]
public class ChatController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IMapper _mapper;

    private readonly ApplicationDbContext _dbContext;

     public ChatController(UserManager<User> userManager,IMapper mapper, RoleManager<IdentityRole> roleManager,ApplicationDbContext dbContext,SignInManager<User> signManager)
    { 
        _userManager = userManager;
        _signManager = signManager;
        _dbContext = dbContext;
        _roleManager = roleManager;
        _mapper= mapper;
    }
    
     // GET: users  
   // [Authorize(Roles ="Expert")]
    [HttpGet("chat/expert")]
    public async Task<ActionResult<IEnumerable<User>>> ListChatOfAll()
{
    try
    {
            var userRoleBsiness = await _userManager.GetUsersInRoleAsync("Business");
            var userRoleAdmin = await _userManager.GetUsersInRoleAsync("Admin");
            var userRoleUser = await _userManager.GetUsersInRoleAsync("Expert");
           
             var allUsers = userRoleBsiness.Concat(userRoleUser).ToList();

             return Ok(allUsers);
    }
    catch (Exception)
    {
        return StatusCode(500, "Internal Server Error");
    }
}

   // [Authorize(Roles ="Business")]
    [HttpGet("chat/business")]
    public async Task<ActionResult<IEnumerable<User>>> ListChatOfAllExpert()
{
    try
    {
            var userRoleUser = await _userManager.GetUsersInRoleAsync("Expert");
           
             var allUsers = userRoleUser.ToList();

             return Ok(allUsers);
    }
    catch (Exception)
    {
        return StatusCode(500, "Internal Server Error");
    }
}

//[Authorize(Roles = "Expert, Business")]
[HttpPost("chat/create")]
public async Task<ActionResult<ChatDTO>> CreateChat([FromBody] ChatRequest chatRequest)
{
    try
    {
        // Get the current user ID from the claims
        var currentUser = await _dbContext.Users.FindAsync(chatRequest.CurrentUserId);
        var userTo = await _dbContext.Users.FindAsync(chatRequest.UserToId);
        if (currentUser == null || userTo == null)
        {
            return NotFound("One or both users not found");
        }

        // Check if a chat room already exists for the two users
        var existingChatRoom = await _dbContext.Chats
            .Include(c => c.Messages)
            .AsNoTracking()
            .Where(c => c.UserChats.Any(u => u.UserId == chatRequest.UserToId) && c.UserChats.Any(u => u.UserId == chatRequest.CurrentUserId))
            .FirstOrDefaultAsync();

        if (existingChatRoom != null)
        {
            // Map Chat entity to ChatDTO using AutoMapper
            var existingChatDTO = _mapper.Map<ChatDTO>(existingChatRoom);

            return Ok(existingChatDTO);
        }

        // Create a new chat
        var chat = new Chat
        {
            UserChats = new List<UserChat>
            {
                new UserChat { User = currentUser },
                new UserChat { User = userTo }
            },
            Messages = new List<Message>(), // Assuming you have a Message class
            // Add any additional chat properties here
        };

        _dbContext.Chats.Add(chat);
        await _dbContext.SaveChangesAsync();

        // Map Chat entity to ChatDTO using AutoMapper
        var newChatDTO = _mapper.Map<ChatDTO>(chat);

        return Ok(newChatDTO);
    }
    catch (Exception)
    {
        return StatusCode(500, "Internal Server Error");
    }
}

public class ChatRequest
{
    public string UserToId { get; set; }
    public string CurrentUserId { get; set; }
}


// Inside your ChatController or relevant service class
//[Authorize(Roles = "Expert, Business")]
[HttpPost("chat/all/{current}")]
public async Task<ActionResult<IEnumerable<Chat>>> GetAllChatsForUser(string current)
{
    try
    {
        
        // Get the current user ID from the claims
       var currentUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == current);
        if (currentUser != null)
        {
            // Retrieve all chats where the current user is either the sender or receiver
           var userChatIds = await _dbContext.UserChats
            .Where(uc => uc.UserId == currentUser.Id)
            .Select(uc => uc.ChatId)
            .ToListAsync();


            // Map Chat entities to ChatDTO using AutoMapper
           

            return Ok(userChatIds);
        }
        else
        {
            return StatusCode(400, "Current user ID not found in claims.");
        }
    }
    catch (Exception)
    {
        return StatusCode(500, "Internal Server Error");
    }
}

[HttpGet("{researchId}/company")]
public async Task<ActionResult<Business>> GetBusinessForResearch(int researchId)
{
    try
    {
        var research = await _dbContext.Researches
            .Include(r => r.business) // Include the related business// Include the associated user
            .FirstOrDefaultAsync(r => r.Id == researchId);

        if (research == null || research.business == null)
        {
            return NotFound();
        }

        return research.business; // Return the connected business with its associated user
    }
    catch (Exception ex)
    {
        // Handle exceptions appropriately (e.g., log, return a specific error response)
        return StatusCode(500, $"Internal server error: {ex.Message}");
    }
}


}