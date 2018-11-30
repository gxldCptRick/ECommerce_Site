using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerceSite.DAL.Models;

namespace ECommerceSite.DAL.Services.Implementations
{
    public class StaticProductService : IProductService
    {
        private static IList<Models.Product> Products { get; set; }

        static StaticProductService()
        {
            Products = new List<Models.Product>();
            for (int i = 0; i < 10; i++)
            {
                var product = new Models.Product
                {
                    Name = "Testing",
                    Cost = 100.00m,
                    Details = new List<string>
                    {
                        "Line 1",
                        "Line 2",
                        "Line 3"
                    },
                    ImageUrl = "https://picsum.photos/300/300",
                    Id = i,
                    Summary = "This is just a test but it will help me be able to make the look and feel better for the site"
                };
                Products.Add(product);
            }
        }

        public Models.Product CreateProduct(Models.Product product)
        {
            return product;
        }

        public void DeleteProduct(Models.Product product)
        {
            //do nothing
        }

        public IEnumerable<Models.Product> GetAllProducts()
        {
            return Products;
        }

        public Models.Product GetProductById(int id)
        {
            var product = default(Models.Product);
            if (id >= 0 &&  id < Products.Count)
            {
                product = Products[id];
            }

            return product;
        }

        public void UpdateProduct(Models.Product product)
        {
            var productInList = GetProductById(product.Id);
            if (productInList != null)
            {
                Products[product.Id] = product;
            }
        }
    }
}
