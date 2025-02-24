namespace Evently.Common.Application.Authorization;

public record PermissionResponse(Guid UserId, HashSet<string> Permissions);
