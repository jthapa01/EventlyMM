using FluentValidation;

namespace Evently.Modules.Ticketing.Application.Tickets.CreateTicketBatch;

public sealed class CreateTicketBatchCommandValidator : AbstractValidator<CreateTicketBatchCommand>
{
    public CreateTicketBatchCommandValidator()
    {
        RuleFor(c => c.OrderId).NotEmpty();
    }
}
