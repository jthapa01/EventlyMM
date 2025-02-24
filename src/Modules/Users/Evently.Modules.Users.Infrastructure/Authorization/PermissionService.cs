using Evently.Common.Application.Authorization;
using Evently.Common.Domain;
using Evently.Modules.Users.Application.Users.GetUserPermission;
using MediatR;

namespace Evently.Modules.Users.Infrastructure.Authorization;

internal sealed class PermissionService(ISender sender): IPermissionService
{
    public async Task<Result<PermissionResponse>> GetUserPermissionAsync(string identityId) 
        => await sender.Send(new GetUserPermissionsQuery(identityId));
}
