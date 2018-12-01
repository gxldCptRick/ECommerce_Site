using ECommerceSite.DAL.Models;

namespace ECommerceSite.DAL.Services
{
    public interface ICartService
    {
        CartDTO GetCartForUser(string userId);
        void AddItemToCart(string userId, int productId);
        void UpdateItemInCart(string userId, int productId, int amountToSet);
        void AddSpecificAmountOfItemsToCart(string userId, int productId, int amount);
        void RemoveItemFromCart(string userId, int productId);
        void CheckoutCart(string userId);
    }
}
