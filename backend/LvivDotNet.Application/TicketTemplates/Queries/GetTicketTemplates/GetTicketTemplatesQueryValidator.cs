using FluentValidation;

namespace LvivDotNet.Application.TicketTemplates.Queries.GetTicketTemplates
{
    /// <summary>
    /// Get ticket templates query validation rules.
    /// </summary>
    public class GetTicketTemplatesQueryValidator : AbstractValidator<GetTicketTemplatesQuery>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetTicketTemplatesQueryValidator"/> class.
        /// </summary>
        public GetTicketTemplatesQueryValidator()
        {
            this.RuleFor(c => c.EventId).NotEmpty().NotEqual(0);
        }
    }
}