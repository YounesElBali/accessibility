using Microsoft.AspNetCore.Mvc;
using wdpr_project.Models;

namespace wdpr_project.Services;

public interface IEmailService
{
    public Task<ActionResult> SendEmailsToParticipants(int Id);
    public Task<ActionResult<Expert>> SendEmail(string to, string subject, string body );

}