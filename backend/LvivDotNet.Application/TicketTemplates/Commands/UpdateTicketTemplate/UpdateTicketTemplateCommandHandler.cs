using System.Data;
using System.Threading;
using System.Threading.Tasks;
using LvivDotNet.Application.Interfaces;
using LvivDotNet.Application.TicketTemplates.Models;

namespace LvivDotNet.Application.TicketTemplates.Commands.UpdateTicketTemplate
{
    public class UpdateTicketTemplateCommandHandler : BaseHandler<UpdateTicketTemplateCommand, TicketTemplateModel>
    {
        public UpdateTicketTemplateCommandHandler(IDbConnectionFactory dbConnectionFactory) : base(dbConnectionFactory) { }

        protected override Task<TicketTemplateModel> Handle(UpdateTicketTemplateCommand request, CancellationToken cancellationToken, IDbConnection connection,
            IDbTransaction transaction)
        {
            throw new System.NotImplementedException();
        }
    }
}