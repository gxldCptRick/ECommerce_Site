using System.Collections.Generic;

namespace ECommerceSite.DAL.Services
{
    public interface IProductService
    {
        // READ
        IEnumerable<Models.Product> GetAllProducts();
        Models.Product GetProductById(int id);

        // CREATE
        Models.Product CreateProduct(Models.Product product);

        // UPDATE
        void UpdateProduct(Models.Product product);

        // DELETE
        void DeleteProduct(Models.Product product);
    }
}
