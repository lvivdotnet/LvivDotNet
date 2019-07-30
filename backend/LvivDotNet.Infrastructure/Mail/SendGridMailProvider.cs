using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LvivDotNet.Application.Interfaces.Mail;
using LvivDotNet.Common.Enums;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace LvivDotNet.Infrastructure
{
    public class SendGridMailProvider : IMailProvider
    {
        private readonly SendGridClient _client;

        public SendGridMailProvider(string apiKey)
        {
            _client = new SendGridClient(apiKey);
        }

        public async Task Send(IMailModel model, IEnumerable<IEmail> recipients)
        {
            var message = CreateMessage(model);
            
            var recipientsModel = recipients.Select(r => new EmailAddress(r.Email, r.Name)).ToList();
            
            message.AddTos(recipientsModel);
            
            await _client.SendEmailAsync(message);
        }

        public async Task Send(IMailModel model, IEmail recipient)
        {
            var message = CreateMessage(model);
            
            var recipientModel = new EmailAddress(recipient.Email, recipient.Name);
            
            message.AddTo(recipientModel);
            
            await _client.SendEmailAsync(message);
        }

        private SendGridMessage CreateMessage(IMailModel model)
        {
            var msg = new SendGridMessage();
            var sender = new EmailAddress(model.Sender.Name, model.Sender.Email);
            
            msg.SetFrom(sender);
            msg.SetSubject(model.Subject);
            msg.AddContent(GetMimeType(model.Content.Type), model.Content.Content);
            
            return msg;
        }

        private string GetMimeType(EmailContentTypeEnum typeEnum)
        {
            string type = null;
            
            switch (typeEnum)
            {
                case EmailContentTypeEnum.HTML:
                    type = MimeType.Html;
                    break;
                default:
                    type = MimeType.Text;
                    break;
            }

            return type;
        }
    }
}