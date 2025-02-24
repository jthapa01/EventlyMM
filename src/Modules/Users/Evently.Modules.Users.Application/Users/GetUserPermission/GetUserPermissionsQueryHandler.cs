using System.Data.Common;
using Dapper;
using Evently.Common.Application.Authorization;
using Evently.Common.Application.Data;
using Evently.Common.Application.Messaging;
using Evently.Common.Domain;
using Evently.Modules.Users.Domain.Users;

namespace Evently.Modules.Users.Application.Users.GetUserPermission;

internal sealed class GetUserPermissionsQueryHandler(IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetUserPermissionsQuery, PermissionResponse>
{
    public async Task<Result<PermissionResponse>> Handle(GetUserPermissionsQuery request,
        CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        const string sql =
            $"""
             SELECT DISTINCT
                u.id AS {nameof(UserPermission.UserId)},
                rp.permission_code AS {nameof(UserPermission.Permission)}
             FROM users.users u
             JOIN users.user_roles ur ON ur.user_id = u.id
             JOIN users.roles_permissions rp ON rp.role_name = ur.role_name
             WHERE u.identity_id = @IdentityId
             """;

        List<UserPermission> permissions = (await connection.QueryAsync<UserPermission>(sql, request)).AsList();

        return !permissions.Any()
            ? Result.Failure<PermissionResponse>(UserErrors.NotFound(request.IdentityId))
            : new PermissionResponse(permissions[0].UserId,
                permissions.Select(permission => permission.Permission).ToHashSet());
    }

    public sealed class UserPermission
    {
        public Guid UserId { get; init; }
        public string Permission { get; init; }
    }
}
