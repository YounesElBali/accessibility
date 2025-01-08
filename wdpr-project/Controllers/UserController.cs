using Microsoft.AspNetCore.Mvc;
using wdpr_project.Services;
using wdpr_project.Models;

namespace wdpr_project.Controllers;

[ApiController]
public class UserController : ControllerBase
{

    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    
    //Expert

    [HttpPost("Expert")]
    public async Task<ActionResult<ExpertDetailDTO>> PostExpert(ExpertFullDTO expert)
    {
        return await _userService.CreateExpert(expert);
    }

    [HttpGet("Expert")]
    public async Task<ActionResult<IEnumerable<ExpertBaseDTO>>> ListExperts()
    {
        return await _userService.GetExpertList();
    }

    [HttpGet("Expert/{id}")]
    public async Task<ActionResult<ExpertDetailDTO>> GetExpert(string id)
    {
        return await _userService.GetExpert(id);
    }

    [HttpPut("Expert/{id}")]
    public async Task<ActionResult> UpdateExpert(string id, ExpertFullDTO expert)
    {
        return await _userService.UpdateExpert(id, expert);
    }

    [HttpDelete("Expert/{id}")]
    public async Task<ActionResult> DeleteExpert(string id)
    {
        return await _userService.DeleteExpert(id);
    }
    
    //Business

    [HttpPost("Business")]
    public async Task<ActionResult<BusinessDTO>> PostBusiness(Business business)
    {
        return await _userService.CreateBusiness(business);
    }

    [HttpGet("Business")]
    public async Task<ActionResult<IEnumerable<BusinessDTO>>> ListBusinesss()
    {
        return await _userService.GetBusinessList();
    }

    [HttpGet("Business/{id}")]
    public async Task<ActionResult<BusinessDTO>> GetBusiness(string id)
    {
        return await _userService.GetBusiness(id);
    }

    [HttpPut("Business/{id}")]
    public async Task<ActionResult> UpdateBusiness(string id, Business business)
    {
        return await _userService.UpdateBusiness(id, business);
    }

    [HttpDelete("Business/{id}")]
    public async Task<ActionResult> DeleteBusiness(string id)
    {
        return await _userService.DeleteBusiness(id);
    }
    
    //Admin

    [HttpPost("Admin")]
    public async Task<ActionResult<AdminDTO>> PostAdmin(Admin Admin)
    {
        return await _userService.CreateAdmin(Admin);
    }

    [HttpGet("Admin")]
    public async Task<ActionResult<IEnumerable<AdminDTO>>> ListAdmins()
    {
        return await _userService.GetAdminList();
    }

    [HttpGet("Admin/{id}")]
    public async Task<ActionResult<AdminDTO>> GetAdmin(string id)
    {
        return await _userService.GetAdmin(id);
    }

    [HttpPut("Admin/{id}")]
    public async Task<ActionResult> UpdateAdmin(string id, Admin admin)
    {
        return await _userService.UpdateAdmin(id, admin);
    }

    [HttpDelete("Admin/{id}")]
    public async Task<ActionResult> DeleteAdmin(string id)
    {
        return await _userService.DeleteAdmin(id);
    }
}