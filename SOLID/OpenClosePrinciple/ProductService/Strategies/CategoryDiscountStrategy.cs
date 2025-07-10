using ProductService.Interfaces;
using ProductService.Models;

namespace ProductService.Strategies;

/// <summary>
/// Discount strategy for specific product categories
/// This demonstrates extending functionality without modifying existing code
/// </summary>
public class CategoryDiscountStrategy : IDiscountStrategy
{
    private readonly string _category;
    private readonly decimal _discountPercentage;
    
    public CategoryDiscountStrategy(string category, decimal discountPercentage = 0.10m)
    {
        _category = category;
        _discountPercentage = discountPercentage;
    }
    
    public string Name => $"{_category} Category Discount ({_discountPercentage:P0})";
    
    public decimal CalculateDiscount(Product product)
    {
        return product.Price * _discountPercentage;
    }
    
    public bool IsApplicable(Product product)
    {
        return product.Category.Equals(_category, StringComparison.OrdinalIgnoreCase);
    }
} 