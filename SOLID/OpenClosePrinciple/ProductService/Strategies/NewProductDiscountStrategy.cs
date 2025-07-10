using ProductService.Interfaces;
using ProductService.Models;

namespace ProductService.Strategies;

/// <summary>
/// Discount strategy for new products (created within a specified time period)
/// This demonstrates extending functionality without modifying existing code
/// </summary>
public class NewProductDiscountStrategy : IDiscountStrategy
{
    private readonly int _daysThreshold;
    private readonly decimal _discountPercentage;
    
    public NewProductDiscountStrategy(int daysThreshold = 30, decimal discountPercentage = 0.05m)
    {
        _daysThreshold = daysThreshold;
        _discountPercentage = discountPercentage;
    }
    
    public string Name => $"New Product Discount ({_discountPercentage:P0})";
    
    public decimal CalculateDiscount(Product product)
    {
        return product.Price * _discountPercentage;
    }
    
    public bool IsApplicable(Product product)
    {
        var daysSinceCreation = (DateTime.Now - product.CreatedDate).Days;
        return daysSinceCreation <= _daysThreshold;
    }
} 