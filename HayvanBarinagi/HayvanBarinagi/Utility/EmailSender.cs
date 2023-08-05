using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace HayvanBarinagi.Utility
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            //Email işlemleri burada yapılabilir
            return Task.CompletedTask;
        }
    }
}
