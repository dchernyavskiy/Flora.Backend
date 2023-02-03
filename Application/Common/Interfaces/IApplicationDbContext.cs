using Microsoft.EntityFrameworkCore;

namespace Flora.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}