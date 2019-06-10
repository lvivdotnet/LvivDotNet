using FluentValidation;

namespace LvivDotNet.Application.TicketTemplates.Queries.GetTicketTemplates
{
    public class GetTicketTemplatesQueryValidator : AbstractValidator<GetTicketTemplatesQuery>
    {
        public GetTicketTemplatesQueryValidator()
        {
            RuleFor(c => c.EventId).NotEmpty().NotEqual(0);
        }
    }
}