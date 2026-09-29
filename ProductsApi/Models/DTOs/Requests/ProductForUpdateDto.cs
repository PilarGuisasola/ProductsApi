
namespace ProductsApi.Models.DTOs.Requests
{
    public class ProductForUpdateDto
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
