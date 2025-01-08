using System.Net;
using System.Net.Mail;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using wdpr_project.Data;
using wdpr_project.Models;

namespace wdpr_project.Services;

public class EmailService : IEmailService
{

   private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public EmailService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ActionResult> SendEmailsToParticipants(int Id)
{
    try
    {
        // Retrieve participants with the specified disability from the database
        var participants = _context.Experts
            .Where(e => e.Disabilities != null && e.Disabilities.Any(d => d.Id == Id))
            .ToList();

        // Check if participants is not null
        if (participants != null)
        {
            // Send email to each participant
            foreach (var participant in participants)
            {
                // Console.WriteLine("gggggggggggg"+participant.PersonalData.ToString());
                // // Check if participant's email is not null or empty before sending
                // if (!string.IsNullOrEmpty(participant.PersonalData.Emailaddress))
                // {
                // Console.WriteLine("ggggdddgggggggg");
                //     // Send email
                    await SendEmail("", "Research Invitation", "Your email content goes here");
                
            }
        }

        // Return a successful response
        return await Task.FromResult(new NoContentResult());
    }
    catch (Exception ex)
    {
        // Log the exception or handle it accordingly
        // You might want to log or rethrow the exception based on your application's needs
        Console.WriteLine($"Error in SendEmailsToParticipants: {ex.Message}");
        throw;
    }
}
    public async Task<ActionResult<Expert>> SendEmail(string to, string subject, string body)
    {
        using (var client = new SmtpClient("smtp.outlook.com"))
        {
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("", "");
            client.Port = 587;
            client.EnableSsl = true;

            var message = new MailMessage("", to, subject, body);

            await client.SendMailAsync(message);
        }
        return  new NotFoundResult();
    }

}