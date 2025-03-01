using System.Security.AccessControl;
using Evently.Common.Domain;

namespace Evently.Modules.Attendance.Domain.Tickets;

public static class TicketErrors
{
    public static readonly Error NotFound = Error.Problem("Tickets.NotFound", "Ticket not found.");

    public static readonly Error InvalidCheckIn =
        Error.Problem("Tickets.InvalidCheckIn", "Ticket cannot be checked in.");

    public static readonly Error DuplicateCheckIn =
        Error.Problem("Tickets.DuplicateCheckIn", "Ticket already checked in.");
}
