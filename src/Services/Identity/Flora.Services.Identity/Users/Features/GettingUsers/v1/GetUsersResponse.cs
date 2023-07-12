using BuildingBlocks.Core.CQRS.Queries;
using Flora.Services.Identity.Users.Dtos;
using Flora.Services.Identity.Users.Dtos.v1;

namespace Flora.Services.Identity.Users.Features.GettingUsers.v1;

public record GetUsersResponse(ListResultModel<IdentityUserDto> IdentityUsers);
