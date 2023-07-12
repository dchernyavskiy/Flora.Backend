using BuildingBlocks.Core.Messaging;
using Flora.Services.Identity.Shared.Models;

namespace Flora.Services.Identity.Users.Features.UpdatingUserState.v1.Events.Integration;

public record UserStateUpdated(Guid UserId, UserState OldUserState, UserState NewUserState) : IntegrationEvent;
