using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using wdpr_project.Data;
using wdpr_project.Models;

namespace wdpr_project.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IMapper _mapper;

    private readonly ApplicationDbContext _dbContext;
    private readonly IConfiguration _configuration;

     public AuthService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager,ApplicationDbContext dbContext,SignInManager<User> signManager,IConfiguration configuration)
    { 
        _userManager = userManager;
        _signManager = signManager;
        _dbContext = dbContext;
        _roleManager = roleManager;
        _configuration = configuration;
    }
public async Task<string> Login(User user)
{
    var userData = await _userManager.FindByNameAsync(user.UserName);

    if (userData != null && validateHasher(userData.UserName, userData.Password, user.Password))
    {
        var result = await _signManager.PasswordSignInAsync(userData, userData.Password, isPersistent: true, lockoutOnFailure: false);
        if (result.Succeeded)
        {
            var roles = await _userManager.GetRolesAsync(userData);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, userData.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var token = new JwtSecurityToken(
                issuer: _configuration.GetValue<string>("JwtUrl"),
                audience: _configuration.GetValue<string>("JwtUrl"),
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15), 
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes("awef98awef978haweof8g7aw789efhh789awef8h9awh89efh98f89uawef9j8aw89hefawef")),
                    SecurityAlgorithms.HmacSha256)
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenString;
        }
    }

    return "Invalid gebruikersnaam of wachtwoord";
}


public async Task<ActionResult<ExpertFullDTO>> CreateExpert(Expert expert)
{
    if (!await _roleManager.RoleExistsAsync("Expert"))
    {
        await _roleManager.CreateAsync(new IdentityRole { Name = "Expert" });
    }

    expert.Password = passwordHasher(expert.UserName,expert.Password);

    // Create the user if it doesn't exist
    var result = await _userManager.CreateAsync(expert, expert.Password);
    if (result.Succeeded)
    {
        var createdUser = await _userManager.FindByNameAsync(expert.UserName);
        await _userManager.AddToRoleAsync(createdUser, "Expert");
        
     var expertEntity = new Expert
        {
            ContactByPhone = expert.ContactByPhone,
            ContactByThirdParty = expert.ContactByThirdParty,
            Disabilities = expert.Disabilities.Select(d => new Disability
            {
                Type = d.Type,
                Description = d.Description
            }).ToList(),
            Aids = expert.Aids.Select(a => new DisabilityAid
            {
                Description = a.Description
            }).ToList(),
            PersonalData = new PersonalData
            {
                Firstname = expert.PersonalData.Firstname,
                Middlenames = expert.PersonalData.Middlenames,
                Lastname = expert.PersonalData.Lastname,
                Emailaddress = expert.PersonalData.Emailaddress,
                Phonenumber = expert.PersonalData.Phonenumber,
                Age = expert.PersonalData.Age,
                Address = new Address
                {
                    Adress = expert.PersonalData.Address.Adress,
                    Postcode = expert.PersonalData.Address.Postcode
                }
            }
        };

        _dbContext.Experts.Add(expertEntity);
        await _dbContext.SaveChangesAsync();

        
        var expertDto = _mapper.Map<ExpertFullDTO>(expertEntity);
        return expertDto;

    }
    return null;
}

public async Task<ActionResult<BusinessDTO>> CreateBusiness(Business business)
{
    if (!await _roleManager.RoleExistsAsync("Business"))
    {
        await _roleManager.CreateAsync(new IdentityRole { Name = "Business" });
    }

    business.Password = passwordHasher(business.UserName,business.Password);

    var result = await _userManager.CreateAsync(business, business.Password);
    if (result.Succeeded)
    {
        var createdUser = await _userManager.FindByNameAsync(business.UserName);
        await _userManager.AddToRoleAsync(createdUser, "Business");

        var businessEntity = new Business
        {
            URL = business.URL,
            Name = business.Name,
            Address = business.Address 
        };

        _dbContext.Businesses.Add(businessEntity);
        await _dbContext.SaveChangesAsync();


        var businessDto = _mapper.Map<BusinessDTO>(businessEntity);
        return businessDto;
    }
    else
    {
        return null;
    }
}

    public string passwordHasher(string userName, string password)
    {
        PasswordHasher<string> pw = new PasswordHasher<string>();
        string passwordHashed = pw.HashPassword(userName, password);
        return passwordHashed;
    }

    public bool validateHasher(string userName,string passwordHashed,  string password)
    {
        PasswordHasher<string> pw = new PasswordHasher<string>();
        var verificationResult = pw.VerifyHashedPassword(userName, passwordHashed, password);
         return verificationResult == PasswordVerificationResult.Success;
    }

}