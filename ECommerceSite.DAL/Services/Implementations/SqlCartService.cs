using ECommerceSite.DAL.Models;
using System;
using System.Linq;

namespace ECommerceSite.DAL.Services.Implementations
{
    public class SqlCartService : ICartService
    {

        private SketchyProductsEntities GenerateContext()
        {
            return new SketchyProductsEntities();
        }

        private Cart GetNewestCartForUser(SketchyProductsEntities context, string userId)
        {
            var cartHistory = context.CartHistories.Where(c => c.UserId == userId).Select(c => c.CartId).ToList();
            var currentCart = context.Carts.FirstOrDefault(c => !cartHistory.Contains(c.Id) && c.UserId == userId);
            return currentCart;

        }

        private int GetCartIdFromCurrentCart(SketchyProductsEntities context, string userId, Cart currentCart)
        {
            var cartId = 0;
            if (currentCart is null)
            {
                var cart = new Cart()
                {
                    UserId = userId
                };
                cart = context.Carts.Add(cart);
                context.SaveChanges();
                cartId = cart.Id;
            }
            else
            {
                cartId = currentCart.Id;
            }
            return cartId;
        }

        public void AddItemToCart(string userId, int productId)
        {
            AddSpecificAmountOfItemsToCart(userId, productId, 1);
        }

        public void AddSpecificAmountOfItemsToCart(string userId, int productId, int amount)
        {
            if (amount < 1) throw new ArgumentOutOfRangeException(nameof(amount), "The amount passed in must be a positive number.");
            using (var context = GenerateContext())
            {
                var currentCart = GetNewestCartForUser(context, userId);
                var cartId = GetCartIdFromCurrentCart(context, userId, currentCart);
                var itemInCart = context.CartDetails.SingleOrDefault(cd => cd.CartId == cartId && cd.ProductId == productId);

                if (itemInCart is null)
                {
                    var item = new CartDetail()
                    {
                        Quantity = amount,
                        CartId = cartId,
                        ProductId = productId,
                    };
                    context.CartDetails.Add(item);
                }
                else
                {
                    itemInCart.Quantity += amount;
                }
                context.SaveChanges();
            }
        }

        public void UpdateItemInCart(string userId, int productId, int amountSetting)
        {
            if (amountSetting < 0) throw new ArgumentOutOfRangeException(nameof(amountSetting), "The amount being set must be 0 or greater.");
            using (var context = GenerateContext())
            {
                var currentCart = GetNewestCartForUser(context, userId);
                if (currentCart is null) throw new ArgumentException("The user must have cart in order to update an item in the cart.");
                var productLine = context.CartDetails.Single(c => c.ProductId == productId && c.CartId == currentCart.Id);
                if (amountSetting == 0) context.CartDetails.Remove(productLine);
                else productLine.Quantity = amountSetting;
                context.SaveChanges();

            }
        }

        public void CheckoutCart(string userId)
        {
            using (var context = GenerateContext())
            {
                var currentCart = GetNewestCartForUser(context, userId);
                if (currentCart is null) throw new ArgumentException("User does not have a cart to checkout with.");
                var history = new CartHistory()
                {
                    CartId = currentCart.Id,
                    UserId = userId,
                    DateSubmited = DateTime.Now,
                };
                context.CartHistories.Add(history);
                context.SaveChanges();
            }
        }

        public CartDTO GetCartForUser(string userId)
        {
            var cart = new CartDTO();
            using (var context = GenerateContext())
            {
                var currentCart = GetNewestCartForUser(context, userId);
                var cartId = GetCartIdFromCurrentCart(context, userId, currentCart);
                cart.UserId = userId;
                foreach (var item in context.CartDetails.Where(cd => cd.CartId == cartId).ToList())
                {
                    var product = context.Products.Single(p => p.ProductId == item.ProductId);
                    var itemData = new CartItem
                    {
                        Amount = item.Quantity,
                        ProductName = product.Name,
                        ProductId = product.ProductId
                    };
                    cart.Cart.Add(itemData);
                    cart.Total += product.Cost * item.Quantity;
                }
            }
            return cart;
        }

        public void RemoveItemFromCart(string userId, int productId)
        {
            using (var context = GenerateContext())
            {
                var cart = GetNewestCartForUser(context, userId);
                var product = context.CartDetails.SingleOrDefault(d => d.CartId == cart.Id && d.ProductId == productId);
                context.CartDetails.Remove(product);
                context.SaveChanges();
            }
        }
    }
}
