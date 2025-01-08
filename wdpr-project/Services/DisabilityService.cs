using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using wdpr_project.Controllers;
using wdpr_project.Data;
using wdpr_project.Models;

namespace wdpr_project.Services;

public class DisabilityService : IDisabilityService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public DisabilityService(ApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    public async Task<ActionResult<DisabilityDTO>> CreateDisability(DisabilityDTO dto)
    {
        Disability disability = new Disability();

        disability.UpdateFields(dto);

        _dbContext.Disabilities.Add(disability);
        await _dbContext.SaveChangesAsync();

        return new CreatedAtActionResult(nameof(GetDisability), nameof(DisabilityController),
            new { id = disability.Id }, _mapper.Map<DisabilityFullDTO>(disability));
    }

    public async Task<ActionResult<IEnumerable<DisabilityDTO>>> GetDisabilityList()
    {
        if (_dbContext.Disabilities is null)
        {
            return new NotFoundResult();
        }

        return await _dbContext.Disabilities
            .ProjectTo<DisabilityDTO>(_mapper.ConfigurationProvider).ToListAsync();
    }

    public async Task<ActionResult<DisabilityFullDTO>> GetDisability(int id)
    {
        if (_dbContext.Disabilities is null)
        {
            return new NotFoundResult();
        }

        var disability = await _dbContext.Disabilities
            .Include(d => d.DisabledExperts)
            .FirstOrDefaultAsync(d => d.Id == id);

        if (disability is null)
        {
            return new NotFoundResult();
        }

        return _mapper.Map<DisabilityFullDTO>(disability);
    }

    public async Task<ActionResult> UpdateDisability(int id, DisabilityFullDTO dto)
    {
        if (id != dto.Id)
        {
            return new BadRequestResult();
        }

        var disability = await _dbContext.Disabilities
            .Include(d => d.DisabledExperts)
            .FirstOrDefaultAsync(d => d.Id == id);

        if (disability is null)
        {
            return new NotFoundResult();
        }

        List<string> disabledExpertsIds = disability.DisabledExperts.Select(e => e.Id).ToList();
        if (
              disabledExpertsIds.Count == dto.DisabledExpertIds.Count) // Check for equality of disabledExperts, modification of those is not allowed via this endpoint
        {
            return new ConflictObjectResult("Modification of the DisabledExperts property is not allowed via this endpoint, please update the relevant expert(s) at /api/Expert/{id}");
        }
        
        disability.UpdateFields(dto);

        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!(_dbContext.Disabilities?.Any(d => d.Id == id)).GetValueOrDefault())
            {
                return new NotFoundResult();
            }
            else
            {
                throw;
            }
        }

        return new NoContentResult();
    }

    public async Task<ActionResult> DeleteDisability(int id)
    {
        if (_dbContext.Disabilities is null)
        {
            return new NotFoundResult();
        }

        var disability = await _dbContext.Disabilities
            .Include(d => d.DisabledExperts)
            .FirstOrDefaultAsync(d => d.Id == id);

        if (disability is null)
        {
            return new NotFoundResult();
        }

        if (disability.DisabledExperts.Count > 0)
        {
            return new ConflictObjectResult(
                "This disability is still listed with some experts, and therefor cannot be deleted.");
        }

        _dbContext.Disabilities.Remove(disability);
        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!(_dbContext.Disabilities?.Any(d => d.Id == id)).GetValueOrDefault())
            {
                return new NotFoundResult();
            }
            else
            {
                throw;
            }
        }

        return new NoContentResult();
    }

    public async Task<ActionResult<DisabilityAidDTO>> CreateDisabilityAid(DisabilityAidDTO dto)
    {
        DisabilityAid aid = new DisabilityAid();

        aid.UpdateFields(dto);

        _dbContext.DisabilityAids.Add(aid);
        await _dbContext.SaveChangesAsync();

        return new CreatedAtActionResult(nameof(GetDisabilityAid), nameof(DisabilityController),
            new { id = aid.Id }, _mapper.Map<DisabilityAidFullDTO>(aid));
    }

    public async Task<ActionResult<IEnumerable<DisabilityAidDTO>>> GetDisabilityAidList()
    {
        if (_dbContext.DisabilityAids is null)
        {
            return new NotFoundResult();
        }

        return await _dbContext.DisabilityAids
            .ProjectTo<DisabilityAidDTO>(_mapper.ConfigurationProvider).ToListAsync();
    }

    public async Task<ActionResult<DisabilityAidFullDTO>> GetDisabilityAid(int id)
    {
        if (_dbContext.DisabilityAids is null)
        {
            return new NotFoundResult();
        }

        var aid = await _dbContext.DisabilityAids
            .Include(a => a.AidUsers)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (aid is null)
        {
            return new NotFoundResult();
        }

        return _mapper.Map<DisabilityAidFullDTO>(aid);
    }

    public async Task<ActionResult> UpdateDisabilityAid(int id, DisabilityAidFullDTO dto)
    {
        
        if (id != dto.Id)
        {
            return new BadRequestResult();
        }

        var aid = await _dbContext.DisabilityAids
            .Include(a => a.AidUsers)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (aid is null)
        {
            return new NotFoundResult();
        }

        List<string> AidUsersIds = aid.AidUsers.Select(e => e.Id).ToList();
        if (
              AidUsersIds.Count == dto.AidUserIds.Count) // Check for equality of disabledExperts, modification of those is not allowed via this endpoint
        {
            return new ConflictObjectResult("Modification of the AidUsers property is not allowed via this endpoint, please update the relevant expert(s) at /api/Expert/{id}");
        }
        
        aid.UpdateFields(dto);

        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!(_dbContext.DisabilityAids?.Any(a => a.Id == id)).GetValueOrDefault())
            {
                return new NotFoundResult();
            }
            else
            {
                throw;
            }
        }

        return new NoContentResult();
    }

    public async Task<ActionResult> DeleteDisabilityAid(int id)
    {
        if (_dbContext.DisabilityAids is null)
        {
            return new NotFoundResult();
        }

        var aid = await _dbContext.DisabilityAids
            .Include(a => a.AidUsers)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (aid is null)
        {
            return new NotFoundResult();
        }

        if (aid.AidUsers.Count > 0)
        {
            return new ConflictObjectResult(
                "This disabilityAid is still being used by some experts, and therefor cannot be deleted.");
        }

        _dbContext.DisabilityAids.Remove(aid);
        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!(_dbContext.DisabilityAids?.Any(a => a.Id == id)).GetValueOrDefault())
            {
                return new NotFoundResult();
            }
            else
            {
                throw;
            }
        }

        return new NoContentResult();
    }
}