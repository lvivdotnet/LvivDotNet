using System.Collections.Generic;
using System.Threading.Tasks;

namespace LvivDotNet.Application.Interfaces.Mail
{
    public interface IMailProvider
    {
        Task<bool> Send(IMailModel model, IEnumerable<IEmail> recipients);
        Task<bool> Send(IMailModel model, IEmail recipient);
    }
}