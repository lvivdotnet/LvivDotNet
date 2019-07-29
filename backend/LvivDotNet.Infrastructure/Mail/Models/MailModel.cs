using LvivDotNet.Application.Interfaces.Mail;

namespace LvivDotNet.Infrastructure.Mail.Models
{
    public class MailModel: IMailModel
    {
        public IEmail Sender { get; set; }
        public string Subject { get; set; }
        public IContent Content { get; set; }
    }
}