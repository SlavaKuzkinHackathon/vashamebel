using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace vashamebel.Models.Admin
{
    public interface IDataRepository
    {

        Product GetProduct(long id);

        IEnumerable<Product> GetAllProducts();

        IEnumerable<Product> GetFilteredProducts(string category = null,
           decimal? price = null);

        void CreateProduct(Product newProduct);

        void UpdateProduct(Product changedProduct, Product originalProduct = null);

        void DeleteProduct(long id);
        
    }
}