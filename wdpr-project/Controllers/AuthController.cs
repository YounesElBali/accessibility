using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using wdpr_project.Models;
using wdpr_project.Services;

[AllowAnonymous]
[ApiController]
public class AurhorizatiomController : ControllerBase
{
    private readonly IAuthService _authService;

     public AurhorizatiomController(IAuthService authService)
    { 
        _authService = authService;
    }
    
    [HttpPost("Login")]
    public async Task<string> Login([FromBody] User user)
    {
    return await _authService.Login(user);
    }

    [HttpPost("create")] 
    public async Task<ActionResult<ExpertFullDTO>> CreateExpert(Expert expert)
    {
    return await _authService.CreateExpert(expert);
    }

    [HttpPost("create-Business")] 
    public async Task<ActionResult<BusinessDTO>> CreateBusiness(Business business)
    {
    return await _authService.CreateBusiness(business);
    }

}