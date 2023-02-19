using System.Text.Json;
using Flora.Application.Common.Interfaces;
using Flora.Application.Plants.Common;
using Flora.Domain.Entities;
using Microsoft.AspNetCore.DataProtection;

namespace Flora.WebApi.Services;

public class BasketService : IBasketService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IDataProtector _dataProtector;

    public BasketService(IHttpContextAccessor httpContextAccessor, IDataProtectionProvider dataProtectionProvider)
    {
        _httpContextAccessor = httpContextAccessor;
        _dataProtector = dataProtectionProvider.CreateProtector(nameof(Basket));
    }
    
    public List<BasketItemBriefDto> GetBasketItems()
    {
        if (_httpContextAccessor.HttpContext.Request.Cookies.TryGetValue(nameof(Basket), out string basketItemsJson))
        {
            var unprotectedBasketItemsJson = _dataProtector.Unprotect(basketItemsJson);
            return JsonSerializer.Deserialize<List<BasketItemBriefDto>>(unprotectedBasketItemsJson)
                   ?? new List<BasketItemBriefDto>();
        }

        return new List<BasketItemBriefDto>();
    }

    public void AddBasketItem(BasketItemBriefDto item)
    {
        var basketItems = GetBasketItems();
        var foundItem = basketItems.FirstOrDefault(x => x.PlantId == item.PlantId);
        if (foundItem == null)
            basketItems.Add(item);
        else if (foundItem.Quantity != item.Quantity)
            foundItem.Quantity = item.Quantity;

        SaveBasketItems(basketItems);
    }

    public void AddBasketItems(IEnumerable<BasketItemBriefDto> items)
    {
        var itemsList = items.ToList();
        itemsList.AddRange(GetBasketItems());
        SaveBasketItems(itemsList);
    }

    private void SaveBasketItems(List<BasketItemBriefDto> basketItems)
    {
        var unprotectedBasketItemsJson = JsonSerializer.Serialize(basketItems);
        var basketItemsJson = _dataProtector.Protect(unprotectedBasketItemsJson);
        _httpContextAccessor.HttpContext?.Response.Cookies.Append(nameof(Basket), basketItemsJson, new CookieOptions()
        {
            Path = "/",
            Expires = DateTime.Now.AddDays(100),
        });
    }

    public void RemoveBasketItem(BasketItemBriefDto item)
    {
        var basketItems = GetBasketItems();
        var foundedItem = basketItems.FirstOrDefault(x => x.PlantId == item.PlantId);
        if (foundedItem != null)
            basketItems.Remove(foundedItem);
        
        SaveBasketItems(basketItems);
    }

    public void Clear()
    {
        SaveBasketItems(new List<BasketItemBriefDto>());
    }
}