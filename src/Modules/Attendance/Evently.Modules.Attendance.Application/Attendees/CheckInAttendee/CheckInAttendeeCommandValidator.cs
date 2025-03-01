using FluentValidation;

namespace Evently.Modules.Attendance.Application.Attendees.CheckInAttendee;

public sealed class CheckInAttendeeCommandValidator : AbstractValidator<CheckInAttendeeCommand>
{
    public CheckInAttendeeCommandValidator()
    {
        RuleFor(x => x.TicketId)
            .NotEmpty()
            .WithMessage("Ticket ID is required.");
    }
}
