using FluentValidation;

namespace LvivDotNet.Application.TicketTemplates.Queries.GetTicketTemplate
{
    /// <summary>
    /// Get ticket template query validation rules.
    /// </summary>
    public class GetTicketTemplateQueryValidator : AbstractValidator<GetTicketTemplateQuery>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetTicketTemplateQueryValidator"/> class.
        /// </summary>
        public GetTicketTemplateQueryValidator()
        {
            this.RuleFor(c => c.Id).NotEmpty().NotEqual(0);
        }
    }
}
