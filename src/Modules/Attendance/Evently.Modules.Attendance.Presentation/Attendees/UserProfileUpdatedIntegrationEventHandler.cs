using Evently.Common.Application.EventBus;
using Evently.Common.Application.Exceptions;
using Evently.Common.Domain;
using Evently.Module.Users.IntegrationEvents;
using Evently.Modules.Attendance.Application.Attendees.UpdateAttendee;
using MediatR;

namespace Evently.Modules.Attendance.Presentation.Attendees;

internal class UserProfileUpdatedIntegrationEventHandler(ISender sender)
: IntegrationEventHandler<UserProfileUpdatedIntegrationEvent>
{
    public override async Task Handle(UserProfileUpdatedIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
    {
        Result result = await sender.Send(
            new UpdateAttendeeCommand(
                integrationEvent.UserId,
                integrationEvent.FirstName,
                integrationEvent.LastName), cancellationToken);
        
        if (result.IsFailure)
        {
            throw new EventlyException(nameof(UpdateAttendeeCommand), result.Error);
        }
    }
}
