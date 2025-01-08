using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using wdpr_project.Data;
using wdpr_project.Models;
using wdpr_project.Services;

namespace wdpr_project.Controllers_
{
    public class ResearchController : Controller
    {
        private readonly ApplicationDbContext _context;
         private readonly IMapper _mapper;

        public ResearchController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
       // [Authorize(Roles = "Expert, Admin, Business")]
        // GET: Research
        [HttpGet("Research")]
        public async Task<ActionResult<IEnumerable<Research>>> ListResearch()
        {
             if (_context.Researches == null)
          {
              return NotFound();
          }
            return await _context.Researches.ToListAsync();
        }
      [HttpGet("Research/{id}")]
        public async Task<ActionResult<Research>> ListResearchById(int id)
        {
            Research research = await _context.Researches.FirstOrDefaultAsync(r => r.Id == id);

            if (research != null)
            {
                return research;  // Return the specific research item.
            }
            else
            {
                return NotFound();  // Return a 404 response if the research item is not found.
            }
        }


[HttpPost("research/{researchId}/participate")]
public async Task<ActionResult> ParticipateInResearch(int researchId, [FromBody] ExpertDTO expert)
{
    try
    {
        // Find the research
        var research = await _context.Researches.FindAsync(researchId);

        if (research == null)
        {
            return NotFound("Onderzoek niet gevonden");
        }

        // Find the expert (assuming you have a way to identify the expert, like using expertId)
        var existingExpert = await _context.Experts.FindAsync(expert.Id);

        if (existingExpert == null)
        {
            return NotFound("Ervaringsdeskundige niet gevonden.");
        }
        research.ResearchExperts ??= new List<ResearchExpert>();

        // Check if the expert is already participating in the research
        if (research.ResearchExperts.Any(e => e.ExpertId == existingExpert.Id))
        {
            return BadRequest("Ervaringsdeskundige heeft zich al aangemeld aan het onderzoek");
        }

        var researchExpert = new ResearchExpert
        {
            Research = research,
            Expert = existingExpert
        };

        // Add the expert to the research
        research.ResearchExperts.Add(researchExpert);

        // Update the database
        await _context.SaveChangesAsync();

        return Ok("Ervaringsdeskundige is toegevoegd aan het onderzoek");
    }
    catch (Exception ex)
    {
        // Handle exceptions appropriately (e.g., log, return a specific error response)
        return StatusCode(500, $" server error: {ex.Message}");
    }
}
public class ExpertDTO{
    public string Id{get;set;}
}
//[Authorize(Roles = "Business")]
[HttpPost("researchess")]
public async Task<ActionResult<IEnumerable<Research>>> GetResearchesWithParticipants([FromBody] CurrentUser currentUser)
{
  try
    {
        // Get the ID of the current business account (adjust this based on your authentication mechanism)
       // var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        // var bedrijfId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
       // Assuming userId is the current user's ID
        var business = await _context.Businesses.FirstOrDefaultAsync(b => b.Id == currentUser.CurrentUserId);
        bool value = false;
     Console.WriteLine(value);
      if (business != null && business.Id != null)
        {
            // Retrieve the researches for the current business
            var researches = await _context.Researches
                .Include(r => r.ResearchExperts)
                    .ThenInclude(re => re.Expert)
                .Where(r => r.business.Id == business.Id)
                .Select(r => new ResearchDTO
                {
                    Id = r.Id,
                    Title = r.Title,
                    Description = r.Description,
                    Reward = r.Reward,
                    Capacity = r.Capacity,
                    Status = r.Status,
                    businessId = r.business.Id,
                    ExpertIds = r.ResearchExperts.Select(re => re.Expert.UserName).ToList()
                })
                .ToListAsync();

            return Ok(researches);
        }
        else
        {
            throw new Exception("Bedrijf niet gevonden");
        }


        // Handle the case where business or business.Id is null
        return NotFound("Business not found or invalid business ID.");

    }
    catch (Exception ex)
    {
        // Handle exceptions appropriately (e.g., log, return a specific error response)
        return StatusCode(500, $"Internal server error: {ex.Message}");
    }
}
public class CurrentUser{
    public string CurrentUserId{get;set;}
}

        [HttpGet("Research/Details/{id}")]
        public async Task<IActionResult> GetResearchDetails(int id)
        {
            var research = await _context.Researches
                .FirstOrDefaultAsync(m => m.Id == id);

            if (research == null)
            {
                return NotFound();
            }

            return Ok(research);
        }

        [HttpGet("Create-Research")]
        public IActionResult Create()
        {
            return Ok();
        }
//[Authorize(Roles = "Business")]
[HttpPost("Create-Research")]
public async Task<ActionResult<ResearchDTO>> CreateResearch([FromBody] ResearchDTO researchdto)
{  
    try
    {
        var business = await _context.Businesses.FirstOrDefaultAsync(b => b.Id == researchdto.businessId);
       
        if (business == null)
        {
            return NotFound("Business not found"); // Adjust this according to your error handling strategy
        }


        var research = new Research
        {
            Id = researchdto.Id,
            Title = researchdto.Title,
            Description = researchdto.Description,
            Reward = researchdto.Reward, 
            Capacity = researchdto.Capacity,
            Status = researchdto.Status,
            business = business, 
            ResearchCriterium = researchdto.ResearchCriterium
        };

        _context.Researches.Add(research);
        //_context.ResearchCriteria.Add(researchCriteria);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetOnderzoek", new { id = research.Id }, research);
    }
    catch (Exception ex)
    {
        // Log the exception or return an appropriate error response
        return StatusCode(500, $"Internal server error: {ex.Message}");
    }
}

       // [Authorize(Roles = "Business")]
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> GetResearchForEdit(int id)
        {
            var research = await _context.Researches.FindAsync(id);

            if (research == null)
            {
                return NotFound();
            }

            return Ok(research);
        }

        // POST: Research/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
      //  [Authorize(Roles = "Business")]
        public async Task<IActionResult> Edit(int id, [FromBody] Research research)
        {
            if (id != research.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(research);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ResearchExists(research.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return Ok(research);
        }

        // GET: Research/Delete/5
       // [Authorize(Roles = "Business")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Researches == null)
            {
                return NotFound();
            }

            var research = await _context.Researches
                .FirstOrDefaultAsync(m => m.Id == id);
            if (research == null)
            {
                return NotFound();
            }

            return Ok(research);
        }
       // [Authorize(Roles = "Business")]
        // POST: Research/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Researches == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Researches'  is null.");
            }
            var research = await _context.Researches.FindAsync(id);
            if (research != null)
            {
                _context.Researches.Remove(research);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ResearchExists(int id)
        {
          return (_context.Researches?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
