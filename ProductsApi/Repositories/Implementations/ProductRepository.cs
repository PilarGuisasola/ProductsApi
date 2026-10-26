using Microsoft.EntityFrameworkCore;
using ProductsApi.Data;
using ProductsApi.Entities;
using ProductsApi.Repositories.Interfaces;

namespace ProductsApi.Repositories.Implementations
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductsApiContext _context;

        public ProductRepository(ProductsApiContext context)
        {
            _context = context;
        }

        public List<Product> GetAllProducts()
        {
            return _context.Products
                .AsNoTracking()
                .ToList();
        }

        public Product? GetProductById(int id)
        {
            return _context.Products
                .FirstOrDefault(p => p.Id == id);
        }

        public void AddProduct(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public void UpdateProduct(Product product)
        {
            var existing = GetProductById(product.Id);

            if (existing != null)
            {
                existing.Name = product.Name;
                existing.Price = product.Price;

                _context.SaveChanges();
            }
        }

        public void DeleteProduct(Product product)
        {
            _context.Products.Remove(product);
            _context.SaveChanges();
        }

        public List<Product> SearchProductsByName(string name)
        {
            return _context.Products
                .AsNoTracking()
                .ToList()
                .Where(p => p.Name.Contains(
                    name, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
    }
}