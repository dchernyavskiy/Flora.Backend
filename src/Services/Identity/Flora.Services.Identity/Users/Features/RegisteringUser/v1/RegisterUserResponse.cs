using Flora.Services.Identity.Users.Dtos;
using Flora.Services.Identity.Users.Dtos.v1;

namespace Flora.Services.Identity.Users.Features.RegisteringUser.v1;

internal record RegisterUserResponse(IdentityUserDto? UserIdentity);
