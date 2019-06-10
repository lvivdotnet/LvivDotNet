using FluentValidation;

namespace LvivDotNet.Application.TicketTemplates.Queries.GetTicketTemplate
{
    public class GetTicketTemplateQueryValidator : AbstractValidator<GetTicketTemplateQuery>
    {
        public GetTicketTemplateQueryValidator()
        {
            RuleFor(c => c.Id).NotEmpty().NotEqual(0);
        }
    }
}
