using Evently.Common.Application.Messaging;

namespace Evently.Modules.Attendance.Application.Attendees.UpdateAttendee;

public record UpdateAttendeeCommand(Guid AttendeeId, string FirstName, string LastName) : ICommand;
