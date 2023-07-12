using Flora.Services.Identity.Identity.Dtos;
using Flora.Services.Identity.Identity.Dtos.v1;

namespace Flora.Services.Identity.Identity.Features.GeneratingRefreshToken.v1;

public record GenerateRefreshTokenResponse(RefreshTokenDto RefreshToken);
