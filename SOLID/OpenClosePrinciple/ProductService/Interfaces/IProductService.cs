using ProductService.Models;

namespace ProductService.Interfaces;

public interface IProductService
{
    IEnumerable<Product> GetAllProducts();
    Product? GetProductById(int id);
    decimal CalculateFinalPrice(Product product);
    IEnumerable<IDiscountStrategy> GetAvailableDiscounts();
} 