using Flora.Application.Common.Models;
using Flora.Application.Plants.Commands.AddToBasket;
using Flora.Application.Plants.Common;

namespace Flora.Application.Common.Interfaces;

public interface IBasketService
{
    //List<BasketItemBriefDto> GetBasketItems();
    List<BasketItemBriefDto> GetBasketItems();
    void AddBasketItem(BasketItemBriefDto item);
    void AddBasketItems(IEnumerable<BasketItemBriefDto> items);
    void RemoveBasketItem(BasketItemBriefDto item);
    void Clear();
}