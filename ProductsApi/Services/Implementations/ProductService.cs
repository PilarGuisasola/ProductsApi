
using ProductsApi.Entities;
using ProductsApi.Models.DTOs.Requests;
using ProductsApi.Models.DTOs.Responses;
using ProductsApi.Repositories.Interfaces;
using ProductsApi.Services.Interfaces;

namespace ProductsApi.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        private ProductForReadDto ToDto(Product product)
        {
            return new ProductForReadDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price
            };
        }

        public List<ProductForReadDto> GetAllProducts()
        {
            return _repository.GetAllProducts()
                .Select(p => ToDto(p))
                .ToList();
        }

        public ProductForReadDto? GetProductById(int id)
        {
            var product = _repository.GetProductById(id);

            return product == null ? null : ToDto(product);
        }

        public ProductForReadDto CreateProduct(ProductForCreateDto dto)
        {
            if (_repository.GetAllProducts().Any(
                p => p.Name.Equals(dto.Name,
                    StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException(
                    "Ya existe un producto con ese nombre");
            }

            var product = new Product
            {
                Name = dto.Name,
                Price = dto.Price
            };

            _repository.AddProduct(product);

            return ToDto(product);
        }

        public void UpdateProduct(int id, ProductForUpdateDto dto)
        {
            var product = _repository.GetProductById(id);

            if (product == null)
                throw new KeyNotFoundException();

            if (_repository.GetAllProducts().Any(
                p => p.Id != id &&
                p.Name.Equals(dto.Name,
                    StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException(
                    "Ya existe un producto con ese nombre");
            }

            product.Name = dto.Name;
            product.Price = dto.Price;

            _repository.UpdateProduct(product);
        }

        public void DeleteProduct(int id)
        {
            var product = _repository.GetProductById(id);

            if (product == null)
                throw new KeyNotFoundException();

            _repository.DeleteProduct(product);
        }

        public List<ProductForReadDto> SearchProductsByName(string name)
        {
            return _repository.SearchProductsByName(name)
                .Select(p => ToDto(p))
                .ToList();
        }

        public ProductStatsDto GetStats()
        {
            var products = _repository.GetAllProducts();

            if (products.Count == 0)
            {
                return new ProductStatsDto
                {
                    Total = 0,
                    AveragePrice = 0,
                    MostExpensiveName = string.Empty
                };
            }

            return new ProductStatsDto
            {
                Total = products.Count,
                AveragePrice = products.Average(p => p.Price),
                MostExpensiveName = products
                    .OrderByDescending(p => p.Price)
                    .First().Name
            };
        }
    }
}
