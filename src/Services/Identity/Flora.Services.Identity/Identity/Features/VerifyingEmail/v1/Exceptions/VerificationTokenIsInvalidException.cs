using BuildingBlocks.Core.Exception.Types;

namespace Flora.Services.Identity.Identity.Features.VerifyingEmail.v1.Exceptions;

public class VerificationTokenIsInvalidException : BadRequestException
{
    public VerificationTokenIsInvalidException(string userId)
        : base($"verification token is invalid for userId '{userId}'.") { }
}
