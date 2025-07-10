using ProductService.Interfaces;
using ProductService.Models;

namespace ProductService.Strategies;

/// <summary>
/// Discount strategy for products that are on sale
/// This demonstrates extending functionality without modifying existing code
/// </summary>
public class SaleDiscountStrategy : IDiscountStrategy
{
    private readonly decimal _discountPercentage;
    
    public SaleDiscountStrategy(decimal discountPercentage = 0.15m) // 15% default
    {
        _discountPercentage = discountPercentage;
    }
    
    public string Name => $"Sale Discount ({_discountPercentage:P0})";
    
    public decimal CalculateDiscount(Product product)
    {
        return product.Price * _discountPercentage;
    }
    
    public bool IsApplicable(Product product)
    {
        return product.IsOnSale;
    }
} 