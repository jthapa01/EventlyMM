using Evently.Common.Application.Authorization;
using Evently.Common.Application.Messaging;

namespace Evently.Modules.Users.Application.Users.GetUserPermission;

public sealed record GetUserPermissionsQuery(string IdentityId): IQuery<PermissionResponse>;
