using LvivDotNet.Common.Enums;

namespace LvivDotNet.Application.Interfaces.Mail
{
    public interface IContent
    {
        EmailContentTypeEnum Type { get; set; }
        string Content { get; set; }
    }
}