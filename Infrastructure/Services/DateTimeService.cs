using Flora.Application.Common.Interfaces;

namespace Flora.Infrastructure.Services;

public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.Now;
}
