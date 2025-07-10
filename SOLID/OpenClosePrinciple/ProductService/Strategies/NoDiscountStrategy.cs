using ProductService.Interfaces;
using ProductService.Models;

namespace ProductService.Strategies;

/// <summary>
/// Base discount strategy - no discount applied
/// This is the default strategy that applies to all products
/// </summary>
public class NoDiscountStrategy : IDiscountStrategy
{
    public string Name => "No Discount";
    
    public decimal CalculateDiscount(Product product)
    {
        return 0; // No discount
    }
    
    public bool IsApplicable(Product product)
    {
        return true; // Applies to all products by default
    }
} 