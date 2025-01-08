using Microsoft.AspNetCore.Mvc;
using wdpr_project.Models;

namespace wdpr_project.Services;

public interface IDisabilityService
{

    public Task<ActionResult<DisabilityDTO>> CreateDisability(DisabilityDTO dto);
    public Task<ActionResult<IEnumerable<DisabilityDTO>>> GetDisabilityList();
    public Task<ActionResult<DisabilityFullDTO>> GetDisability(int id);
    public Task<ActionResult> UpdateDisability(int id, DisabilityFullDTO dto);
    public Task<ActionResult> DeleteDisability(int id);

    public Task<ActionResult<DisabilityAidDTO>> CreateDisabilityAid(DisabilityAidDTO dto);
    public Task<ActionResult<IEnumerable<DisabilityAidDTO>>> GetDisabilityAidList();
    public Task<ActionResult<DisabilityAidFullDTO>> GetDisabilityAid(int id);
    public Task<ActionResult> UpdateDisabilityAid(int id, DisabilityAidFullDTO dto);
    public Task<ActionResult> DeleteDisabilityAid(int id);

    
}