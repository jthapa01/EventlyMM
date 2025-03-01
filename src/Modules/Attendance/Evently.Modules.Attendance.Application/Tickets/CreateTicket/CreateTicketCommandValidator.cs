using FluentValidation;

namespace Evently.Modules.Attendance.Application.Tickets.CreateTicket;

public class CreateTicketCommandValidator : AbstractValidator<CreateTicketCommand>
{
    public CreateTicketCommandValidator()
    {
        RuleFor(x => x.TicketId)
            .NotEmpty();
        
        RuleFor(x => x.AttendeeId)
            .NotEmpty();
        
        RuleFor(x => x.EventId)
            .NotEmpty();

        RuleFor(x => x.Code)
            .NotEmpty();
    }
}
