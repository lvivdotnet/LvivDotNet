using LvivDotNet.Application.Interfaces.Mail;
using LvivDotNet.Common.Enums;

namespace LvivDotNet.Infrastructure.Mail.Models
{
    public class ContentModel : IContent
    {
        public EmailContentTypeEnum Type { get; set; }
        public string Content { get; set; }
    }
}