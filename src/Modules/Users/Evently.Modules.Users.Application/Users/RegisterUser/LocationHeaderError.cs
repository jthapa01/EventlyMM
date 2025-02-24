using Evently.Common.Domain;

namespace Evently.Modules.Users.Application.Users.RegisterUser;

public static class LocationHeaderError
{
    public static readonly Error LocationHeaderMissing = Error.Failure(
        "Identity.EmailIsNotUnique",
        "The specified email is not unique");
}
