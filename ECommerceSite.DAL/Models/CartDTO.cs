using System.Collections.Generic;
using System.Linq;

namespace ECommerceSite.DAL.Models
{
    public class CartDTO
    {
        //cart to store all the data for the current info
        //the key is the product id and 
        //the value is the amount for that specific product
        private List<CartItem> _cart;
        public List<CartItem> Cart
        {
            get => _cart;
            private set => _cart = value;
        }

        public decimal Total { get; set; }
        public string UserId { get; set; }

        public CartDTO()
        {
            Cart = new List<CartItem>();
        }


    }
}
