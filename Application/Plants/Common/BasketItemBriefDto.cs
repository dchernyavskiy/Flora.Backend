namespace Flora.Application.Plants.Common;

public record BasketItemBriefDto
{
    public Guid PlantId { get; set; }
    public int Quantity { get; set; }
}