using FluentValidation;

namespace Evently.Modules.Attendance.Application.Attendees.UpdateAttendee;

public sealed class UpdateAttendeeCommandValidator : AbstractValidator<UpdateAttendeeCommand>
{
    public UpdateAttendeeCommandValidator()
    {
        RuleFor(x => x.AttendeeId)
            .NotEmpty()
            .NotEqual(Guid.Empty);

        RuleFor(x => x.FirstName)
            .NotEmpty();

        RuleFor(x => x.LastName)
            .NotEmpty();
    }
}
