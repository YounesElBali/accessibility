using Microsoft.AspNetCore.Mvc;
using wdpr_project.Services;
using wdpr_project.Models;

namespace wdpr_project.Controllers;

[ApiController]
public class DisabilityController : ControllerBase
{

    private readonly IDisabilityService _disabilityService;

    public DisabilityController(IDisabilityService disabilityService)
    {
        _disabilityService = disabilityService;
    }
    
    //Disability

    /// <summary>
    /// 
    /// </summary>
    /// <param name="disability"></param>
    /// <returns>A newly created disability</returns>
    [HttpPost("Disability")]
    public async Task<ActionResult<DisabilityDTO>> PostDisability(DisabilityDTO disability)
    {
        return await _disabilityService.CreateDisability(disability);
    }

    [HttpGet("Disability")]
    public async Task<ActionResult<IEnumerable<DisabilityDTO>>> ListDisabilities()
    {
        return await _disabilityService.GetDisabilityList();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("Disability/{id}")]
    public async Task<ActionResult<DisabilityFullDTO>> GetDisability(int id)
    {
        return await _disabilityService.GetDisability(id);
    }

    [HttpPut("Disability/{id}")]
    public async Task<ActionResult> UpdateDisability(int id, DisabilityFullDTO disability)
    {
        return await _disabilityService.UpdateDisability(id, disability);
    }

    /// <summary>
    /// Deletes a specific disability
    /// </summary>
    /// <param name="id"></param>
    /// <response code="204">No response if the disability is successfully deleted</response>
    /// <response code="404">If the disability is not found in the database</response>
    /// <response code="409">If the disability cannot be deleted, because it is still in active use</response>
    [HttpDelete("Disability/{id}")]
    public async Task<ActionResult> DeleteDisability(int id)
    {
        return await _disabilityService.DeleteDisability(id);
    }
    
    //DisabilityAid

    [HttpPost("DisabilityAid")]
    public async Task<ActionResult<DisabilityAidDTO>> PostDisabilityAid(DisabilityAidDTO disabilityAid)
    {
        return await _disabilityService.CreateDisabilityAid(disabilityAid);
    }

    [HttpGet("DisabilityAid")]
    public async Task<ActionResult<IEnumerable<DisabilityAidDTO>>> ListDisabilityAids()
    {
        return await _disabilityService.GetDisabilityAidList();
    }

    [HttpGet("DisabilityAid/{id}")]
    public async Task<ActionResult<DisabilityAidFullDTO>> GetDisabilityAid(int id)
    {
        return await _disabilityService.GetDisabilityAid(id);
    }

    [HttpPut("DisabilityAid/{id}")]
    public async Task<ActionResult> UpdateDisabilityAid(int id, DisabilityAidFullDTO disabilityAid)
    {
        return await _disabilityService.UpdateDisabilityAid(id, disabilityAid);
    }

    [HttpDelete("DisabilityAid/{id}")]
    public async Task<ActionResult> DeleteDisabilityAid(int id)
    {
        return await _disabilityService.DeleteDisabilityAid(id);
    }
}