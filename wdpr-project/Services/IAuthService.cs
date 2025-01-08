using Microsoft.AspNetCore.Mvc;
using wdpr_project.Models;

namespace wdpr_project.Services;

public interface IAuthService
{
    public Task<string> Login( User user);
    public Task<ActionResult<ExpertFullDTO>> CreateExpert(Expert expert);
    public Task<ActionResult<BusinessDTO>> CreateBusiness(Business business);
    public string passwordHasher(string userName, string password);
    public bool validateHasher(string userName,string passwordHashed,  string password);
}