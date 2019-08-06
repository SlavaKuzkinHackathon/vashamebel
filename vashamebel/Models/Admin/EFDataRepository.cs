using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace vashamebel.Models.Admin
{
    public class EFDataRepository : IDataRepository
    {
        private ApplicationContext context;
        IHostingEnvironment _appEnvironment;

        public EFDataRepository(ApplicationContext ctx, IHostingEnvironment appEnvironment)
        {
            context = ctx;
            _appEnvironment = appEnvironment;
        }

        public Product GetProduct(long id)
        {
            return context.Products.Find(id);
        }

        public IEnumerable<Product> GetAllProducts()
        {
            Console.WriteLine("GetAllProducts");
            return context.Products;
        }

        public IEnumerable<Product> GetFilteredProducts(string name = null,
                decimal? price = null)
        {

            IQueryable<Product> data = context.Products;
            if (name != null)
            {
                data = data.Where(p => p.Name == name);
            }
            if (price != null)
            {
                data = data.Where(p => p.Price >= price);
            }
            return data;
        }

        public void CreateProduct(Product newProduct)
        {
            newProduct.Id = 0;
            context.Products.Add(newProduct);
            context.SaveChanges();
            Console.WriteLine($"New Key: {newProduct.Id}");
        }
    

        public void UpdateProduct(Product changedProduct, Product originalProduct = null)
        {
            if (originalProduct == null)
            {
                originalProduct = context.Products.Find(changedProduct.Id);
            }
            else
            {
                context.Products.Attach(originalProduct);
            }
            originalProduct.Name = changedProduct.Name;
            originalProduct.Description = changedProduct.Description;
            originalProduct.Price = changedProduct.Price;
            originalProduct.InQuantity = changedProduct.InQuantity;
            originalProduct.NameImage = changedProduct.NameImage;
            originalProduct.Path = changedProduct.Path;
            context.SaveChanges();
        }

        public void DeleteProduct(long id)
        {
            context.Products.Remove(new Product { Id = id });
            context.SaveChanges();
        }


    }
}