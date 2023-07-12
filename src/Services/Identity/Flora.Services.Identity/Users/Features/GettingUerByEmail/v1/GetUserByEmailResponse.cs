using Flora.Services.Identity.Users.Dtos;
using Flora.Services.Identity.Users.Dtos.v1;

namespace Flora.Services.Identity.Users.Features.GettingUerByEmail.v1;

public record GetUserByEmailResponse(IdentityUserDto? UserIdentity);
