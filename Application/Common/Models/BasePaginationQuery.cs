namespace Flora.Application.Common.Models;

public record BasePaginationQuery
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
};