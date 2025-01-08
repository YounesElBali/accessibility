using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using wdpr_project.Controllers;
using wdpr_project.Data;
using wdpr_project.Models;

namespace wdpr_project.Services;

public class UserService : IUserService
{

    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    public UserService(ApplicationDbContext dbContext, IMapper mapper, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _roleManager = roleManager;
        _userManager = userManager;
    }

//     //Expert
    
    public async Task<ActionResult<ExpertDetailDTO>> CreateExpert(ExpertFullDTO dto)
    {
        Expert expert = new Expert();

        ActionResult? syncError = await expert.UpdateFields(dto, _dbContext);

        if (syncError is not null)
        {
            return syncError;
        }

        _dbContext.Experts.Add(expert);
        await _dbContext.SaveChangesAsync();
        
        return new CreatedAtActionResult(nameof(GetExpert), nameof(UserController),
            new { id = expert.Id }, _mapper.Map<ExpertFullDTO>(expert));
    }
    public async Task<ActionResult<IEnumerable<ExpertBaseDTO>>> GetExpertList()
    {
        if (_dbContext.Experts is null)
        {
            return new NotFoundResult();
        }

        return await _dbContext.Experts
            .ProjectTo<ExpertBaseDTO>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<ActionResult<ExpertDetailDTO>> GetExpert(string id)
    {
        if (_dbContext.Experts is null)
        {
            return new NotFoundResult();
        }

        var expert = await _dbContext.Experts
            .Include(e => e.PersonalData)
            .Include(e => e.Caretaker)
            .Include(e => e.Disabilities).AsSplitQuery()
            .Include(e => e.Aids).AsSplitQuery()
            .FirstOrDefaultAsync(e => e.Id == id);

        if (expert is null)
        {
            return new NotFoundResult();
        }

        return _mapper.Map<ExpertDetailDTO>(expert);
    }

    public async Task<ActionResult> UpdateExpert(string id, ExpertFullDTO dto)
    {
        if (id != dto.Id)
        {
            return new BadRequestResult();
        }
        
        Expert expert = await _dbContext.Experts
            .Include(e => e.PersonalData)
            .ThenInclude(p => p.Address)
            .Include(e => e.Caretaker)
            .ThenInclude(p => p.Address)
            .Include(e => e.Disabilities).AsSplitQuery()
            .Include(e => e.Aids).AsSplitQuery()
            .FirstOrDefaultAsync(a => a.Id == dto.Id);

        if (expert is null)
        {
            return new NotFoundResult();
        }

        ActionResult? syncError = await expert.UpdateFields(dto, _dbContext);

        if (syncError is not null)
        {
            return syncError;
        }

        _dbContext.Entry(expert).State = EntityState.Modified;

        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!(_dbContext.Experts?.Any(e => e.Id == id)).GetValueOrDefault())
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

    public async Task<ActionResult> DeleteExpert(string id)
    {
        if (_dbContext.Experts is null)
        {
            return new NotFoundResult();
        }

        var expert = new Expert(id);
        _dbContext.Experts.Remove(expert); //TODO: deletion propagation
        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!(_dbContext.Experts?.Any(e => e.Id == id)).GetValueOrDefault())
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
    
    //Business

    public async Task<ActionResult<BusinessDTO>> CreateBusiness(Business business)
    {
        _dbContext.Businesses.Add(business);
        await _dbContext.SaveChangesAsync();
        
        return new CreatedAtActionResult(nameof(GetBusiness), nameof(UserController),
            new { id = business.Id }, business);
    }

    public async Task<ActionResult<IEnumerable<BusinessDTO>>> GetBusinessList()
    {
        if (_dbContext.Businesses is null)
        {
            return new NotFoundResult();
        }

        return await _dbContext.Businesses
            .ProjectTo<BusinessDTO>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<ActionResult<BusinessDTO>> GetBusiness(string id)
    {
        if (_dbContext.Businesses is null)
        {
            return new NotFoundResult();
        }

        var business = await _dbContext.Businesses
            .Include(b => b.Address)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (business is null)
        {
            return new NotFoundResult();
        }

        return _mapper.Map<BusinessDTO>(business);
    }

    public async Task<ActionResult> UpdateBusiness(string id, Business business)
    {
        if (id != business.Id)
        {
            return new BadRequestResult();
        }

        _dbContext.Entry(business).State = EntityState.Modified;

        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!(_dbContext.Businesses?.Any(e => e.Id == id)).GetValueOrDefault())
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

    public async Task<ActionResult> DeleteBusiness(string id)
    {
        if (_dbContext.Businesses is null)
        {
            return new NotFoundResult();
        }

        var business = new Business(id);
        _dbContext.Businesses.Remove(business);
        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!(_dbContext.Businesses?.Any(e => e.Id == id)).GetValueOrDefault())
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
    
    //Admin

    public async Task<ActionResult<AdminDTO>> CreateAdmin(Admin admin)
    {
        _dbContext.Admins.Add(admin);
        await _dbContext.SaveChangesAsync();
        
        return new CreatedAtActionResult(nameof(GetAdmin), nameof(UserController),
            new { id = admin.Id }, admin);
    }

    public async Task<ActionResult<IEnumerable<AdminDTO>>> GetAdminList()
    {
        if (_dbContext.Admins is null)
        {
            return new NotFoundResult();
        }

        return await _dbContext.Admins
            .ProjectTo<AdminDTO>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<ActionResult<AdminDTO>> GetAdmin(string id)
    {
        if (_dbContext.Admins is null)
        {
            return new NotFoundResult();
        }

        var admin = await _dbContext.Admins.FindAsync(id);

        if (admin is null)
        {
            return new NotFoundResult();
        }

        return _mapper.Map<AdminDTO>(admin);
    }

    public async Task<ActionResult> UpdateAdmin(string id, Admin admin)
    {
        if (id != admin.Id)
        {
            return new BadRequestResult();
        }

        _dbContext.Entry(admin).State = EntityState.Modified;

        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!(_dbContext.Admins?.Any(e => e.Id == id)).GetValueOrDefault())
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

    public async Task<ActionResult> DeleteAdmin(string id)
    {
        if (_dbContext.Admins is null)
        {
            return new NotFoundResult();
        }

        var admin = new Admin(id);
        _dbContext.Admins.Remove(admin);
        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!(_dbContext.Admins?.Any(e => e.Id == id)).GetValueOrDefault())
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