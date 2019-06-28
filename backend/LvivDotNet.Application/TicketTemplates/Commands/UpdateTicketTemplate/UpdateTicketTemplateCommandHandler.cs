using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using LvivDotNet.Application.Interfaces;
using LvivDotNet.Application.TicketTemplates.Models;
using MediatR;

namespace LvivDotNet.Application.TicketTemplates.Commands.UpdateTicketTemplate
{
    public class UpdateTicketTemplateCommandHandler : BaseHandler<UpdateTicketTemplateCommand>
    {
        public UpdateTicketTemplateCommandHandler(IDbConnectionFactory dbConnectionFactory) : base(dbConnectionFactory) { }

        protected override async Task<Unit> Handle(UpdateTicketTemplateCommand request, CancellationToken cancellationToken, IDbConnection connection,
            IDbTransaction transaction)
        {
            await connection.ExecuteAsync(
                    "update dbo.[ticket_template] " +
                    "set Name = @Name, " +
                    "[From] = @From, " +
                    "[To] = @To, " +
                    "Price = @Price " +
                    "where Id = @Id",
                    request,
                    transaction
                );


            return Unit.Value;
        }
    }
}