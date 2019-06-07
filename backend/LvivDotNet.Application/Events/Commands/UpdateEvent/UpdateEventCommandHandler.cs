using Dapper;
using LvivDotNet.Application.Interfaces;
using MediatR;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace LvivDotNet.Application.Events.Commands.UpdateEvent
{
    public class UpdateEventCommandHandler : BaseHandler<UpdateEventCommand>
    {
        public UpdateEventCommandHandler(IDbConnectionFactory dbConnectionFactory) : base(dbConnectionFactory) { }

        protected override async Task<Unit> Handle(UpdateEventCommand request, CancellationToken cancellationToken, IDbConnection connection, IDbTransaction transaction)
        {
            await connection.ExecuteAsync(
                    "update dbo.[event] " +
                    "set Name = @Name " +
                    "set StartDate = @StartDate " +
                    "set EndDate = @EndDate " +
                    "set Address = @Address " +
                    "set Title = @Title " +
                    "set Description = @Description " +
                    "set MaxAttendees = @MaxAttendees " +
                    "where Id = @Id",
                    request,
                    transaction
                );

            return Unit.Value;
        }
    }
}
