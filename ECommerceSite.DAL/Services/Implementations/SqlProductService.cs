using AutoMapper;
using ECommerceSite.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ECommerceSite.DAL.Services.Implementations
{
    public class SqlProductService : IProductService
    {

        static SqlProductService()
        {
            Mapper.Initialize(config =>
            {
                config.CreateMap<Models.Product, Entities.Models.Product>()
                .ForMember(d => d.ProductId, ops => ops.MapFrom(s => s.Id))
                .ForMember(d => d.ProductDetails, ops => ops.Ignore());
                config.CreateMap<Entities.Models.Product, Models.Product>()
                .ForMember(d => d.Id, ops => ops.MapFrom(s => s.ProductId))
                .ForMember(d => d.Details, ops => ops.Ignore());
            });
        }

        public Models.Product CreateProduct(Models.Product product)
        {
            Models.Product newestProduct = product;
            using (var context = new SketchyProductsEntities())
            {
                var productToAdd = Mapper.Map<Entities.Models.Product>(product);
                var productAdded = context.Products.Add(productToAdd);
                context.SaveChanges();
                newestProduct= Mapper.Map<Models.Product>(productAdded);
            }

            foreach (var detail in product.Details)
            {
                AddProductToModel(newestProduct.Id, detail);
            }
            return newestProduct;
        }

        private void AddProductToModel(int id, string detail)
        {
            using (var context = new SketchyProductsEntities())
            {
                var productDetail = new Entities.Models.ProductDetail
                {
                    ProductId = id,
                    DetailText = detail
                };
                context.ProductDetails.Add(productDetail);
                context.SaveChanges();
            }
        }

        public void DeleteProduct(Models.Product product)
        {
            using (var context = new SketchyProductsEntities())
            {
                var productToDelete = context.Products.Single(s => s.ProductId == product.Id);
                context.Products.Remove(productToDelete);
                context.SaveChanges();
            }
        }

        public IEnumerable<Models.Product> GetAllProducts()
        {
            using (var context = new SketchyProductsEntities())
            {
                foreach (var items in context.Products.ToList())
                {
                    var product = Mapper.Map<Models.Product>(items);
                    foreach (var detail in items.ProductDetails)
                    {
                        product.Details.Add(detail.DetailText);
                    }
                    yield return product;
                }
            }
        }

        public Models.Product GetProductById(int id)
        {
            using (var context = new SketchyProductsEntities())
            {
                var productDb = context.Products.SingleOrDefault(p => p.ProductId == id);
                var product =  Mapper.Map<Models.Product>(productDb);
                foreach (var detail in productDb.ProductDetails)
                {
                    product.Details.Add(detail.DetailText);
                }
                return product;
            }
        }

        public void UpdateProduct(Models.Product product)
        {
            using (var context = new SketchyProductsEntities())
            {
                var productDb = context.Products.SingleOrDefault(p => p.ProductId == product.Id);
                if(productDb != null)
                {
                    productDb.Cost = product.Cost;
                    productDb.ImageUrl = product.ImageUrl;
                    productDb.Name = product.Name;
                    productDb.Summary = product.Summary;
                    context.SaveChanges();
                    foreach (var detail in productDb.ProductDetails.ToList())
                    {
                        context.ProductDetails.Remove(detail);
                    }
                    context.SaveChanges();
                }
            }

            foreach (var detail in product.Details)
            {
                AddProductToModel(product.Id, detail);
            }
        }
    }
}
