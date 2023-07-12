using Flora.Services.Identity.Shared.Models;

namespace Flora.Services.Identity.Users.Features.UpdatingUserState.v1;

public record UpdateUserStateRequest
{
    public UserState UserState { get; init; }
}
