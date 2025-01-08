using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Identity;
using wdpr_project.Models;
using wdpr_project.Data;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;


[ApiController]
public class AdminController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IMapper _mapper;

    private readonly ApplicationDbContext _dbContext;
    private readonly IConfiguration _configuration;

     public AdminController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager,ApplicationDbContext dbContext,SignInManager<User> signManager,IConfiguration configuration)
    { 
        _userManager = userManager;
        _signManager = signManager;
        _dbContext = dbContext;
        _roleManager = roleManager;
        _configuration = configuration;
    }

    //[Authorize(Roles ="Admin")]
      [HttpGet("admin/get/expert")]
        public async Task<ActionResult<IEnumerable<Expert>>> ListExperts()
        {
           var experts = await _dbContext.Experts
        .Include(e => e.PersonalData)
            .ThenInclude(pd => pd.Address)
        .ToListAsync();

             if (experts == null)
          {
              return NotFound();
          }
            return experts;
        }
    
   // [Authorize(Roles ="Admin")]
     [HttpGet("admin/get/business")]
        public async Task<ActionResult<IEnumerable<Business>>> ListBussines()
        {
             var businesses = await _dbContext.Businesses
                .ToListAsync();

            if (businesses == null)
            {
                return NotFound();
            }

            return businesses;
        }
        
   // [Authorize(Roles ="Admin")]
     [HttpGet("admin/get/research")]
public async Task<ActionResult<IEnumerable<Research>>> ListResearch()
{
    var researches = await _dbContext.Researches
        .Include(r => r.ResearchCriterium)
            .ThenInclude(rc => rc.Disability)  // Eager loading ResearchCriterium // Eager loading Disability.Description
        .ToListAsync();

    if (researches == null)
    {
        return NotFound();
    }

    return researches;
}

}