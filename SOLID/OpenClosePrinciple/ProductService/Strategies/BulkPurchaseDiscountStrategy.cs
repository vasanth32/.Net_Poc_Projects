using ProductService.Interfaces;
using ProductService.Models;

namespace ProductService.Strategies;

/// <summary>
/// Example of how easy it is to add new discount strategies without modifying existing code.
/// This demonstrates the "Open for Extension" part of the Open/Closed Principle.
/// </summary>
public class BulkPurchaseDiscountStrategy : IDiscountStrategy
{
    private readonly int _minimumQuantity;
    private readonly decimal _discountPercentage;
    
    public BulkPurchaseDiscountStrategy(int minimumQuantity = 3, decimal discountPercentage = 0.20m)
    {
        _minimumQuantity = minimumQuantity;
        _discountPercentage = discountPercentage;
    }
    
    public string Name => $"Bulk Purchase Discount ({_discountPercentage:P0} for {_minimumQuantity}+ items)";
    
    public decimal CalculateDiscount(Product product)
    {
        // For demonstration, we'll simulate bulk purchase based on product price
        // In a real scenario, this would depend on the actual quantity being purchased
        if (product.Price > 100) // Expensive items get bulk discount
        {
            return product.Price * _discountPercentage;
        }
        return 0;
    }
    
    public bool IsApplicable(Product product)
    {
        // Apply to expensive items (simulating bulk purchase scenario)
        return product.Price > 100;
    }
} 