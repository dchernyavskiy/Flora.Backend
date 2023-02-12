using Flora.Application.Plants.Commands.AddToBasket;
using Flora.Application.Plants.Common;

namespace Flora.Application.Common.Interfaces;

public interface IBasketService
{
    List<BasketItemBriefDto> GetBasketItems();
    void AddBasketItem(BasketItemBriefDto item);
    void RemoveBasketItem(BasketItemBriefDto item);
}