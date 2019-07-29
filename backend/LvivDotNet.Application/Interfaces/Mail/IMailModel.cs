using System.Collections.Generic;
using LvivDotNet.Application.Interfaces.Mail;

namespace LvivDotNet.Application.Interfaces.Mail
{
    public interface IMailModel
    {
        IEmail Sender { get; set; }
        string Subject { get; set; }
        IContent Content { get; set; }
    }
}