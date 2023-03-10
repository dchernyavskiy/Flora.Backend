using AutoMapper;
using Flora.Application.Common.Interfaces;
using Flora.Application.Wishlists.Commands.CreateWishlist;
using Flora.Infrastructure.Persistence;
using Moq;
using FluentAssertions;
using NSubstitute;

namespace Flora.Application.IntegrationTests.Wishlists.Commands;

public class CreateWishlistTests : BaseTestFixture
{
    private ICurrentUserService _currentUserService = Substitute.For<ICurrentUserService>();
    
    [Test]
    public async Task CreateWishlist_AuthorizedUser_MustReturnNotEmptyId()
    {
        var command = new CreateWishlistCommand();
        var result = await SendAsync(command);
        Assert.AreNotEqual(Guid.Empty, result);
    }
    
    
    [Test]
    public async Task CreateWishlist_UnauthorizedUser_ThrowsUnauthorizedAccessException()
    {
        var context = await GetService<ApplicationDbContext>();
        var mapper = await GetService<IMapper>();
        _currentUserService.UserId.Returns(Guid.Empty);
        
        var handler = new CreateWishlistCommandHandler(context, mapper, _currentUserService);

        var command = new CreateWishlistCommand();
        
        Assert.ThrowsAsync<UnauthorizedAccessException>(() => handler.Handle(command, CancellationToken.None));
        
        // var oldUserId = UserIdProvider.Id;
        // UserIdProvider.Id = Guid.Empty;
        // var command = new CreateWishlistCommand();
        //
        // Assert.ThrowsAsync<UnauthorizedAccessException>(() => SendAsync(command));
        //
        // UserIdProvider.Id = oldUserId;
    }
}