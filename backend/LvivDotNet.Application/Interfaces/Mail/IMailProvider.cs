using System.Collections.Generic;
using System.Threading.Tasks;

namespace LvivDotNet.Application.Interfaces.Mail
{
    public interface IMailProvider
    {
        Task Send(IMailModel model, IEnumerable<IEmail> recipients);
        Task Send(IMailModel model, IEmail recipient);
    }
}