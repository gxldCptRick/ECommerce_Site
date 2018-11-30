using System.Collections.Generic;

namespace ECommerceSite.DAL.Models
{
    public class CartDTO
    {
        //cart to store all the data for the current info
        //the key is the product id and 
        //the value is the amount for that specific product
        private Dictionary<string, int> _cart;
        public Dictionary<string, int> Cart
        {
            get => _cart;
            private set => _cart = value;
        }
        public string UserId { get; set; }

        public CartDTO()
        {
            Cart = new Dictionary<string, int>();
        }


    }
}
