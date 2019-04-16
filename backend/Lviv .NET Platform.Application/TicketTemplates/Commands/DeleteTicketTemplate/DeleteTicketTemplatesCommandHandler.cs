using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Lviv_.NET_Platform.Application.Exceptions;
using Lviv_.NET_Platform.Application.Interfaces;
using MediatR;

namespace Lviv_.NET_Platform.Application.TicketTemplates.Commands.DeleteTicketTemplate
{
    public class DeleteTicketTemplatesCommandHandler : BaseHandler<DeleteTicketTemplateCommand>
    {
        public DeleteTicketTemplatesCommandHandler(IDbConnectionFactory dbConnectionFactory) : base(dbConnectionFactory) { }

        protected override async Task<Unit> Handle(DeleteTicketTemplateCommand request, CancellationToken cancellationToken, IDbConnection connection, IDbTransaction transaction)
        {
            var deletedCount = await connection.ExecuteAsync(
                    "delete from dbo.[ticket_template] " +
                    "where Id = @TicketTemplateId",
                    new { TicketTemplateId = request.Id },
                    transaction
                );

            if (deletedCount == 0)
            {
                throw new NotFoundException("Ticket Template", request.Id);
            }

            return Unit.Value;
        }
    }
}
