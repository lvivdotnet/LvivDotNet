using LvivDotNet.Application.Interfaces.Mail;

namespace LvivDotNet.Infrastructure.Mail.Models
{
    public class EmailModel : IEmail
    {
        public string Email { get; set; }
        public string Name { get; set; }

        public EmailModel(string email, string name = null)
        {
            Email = email;
            name = name;
        }
    }
}