using ProductService.Interfaces;
using ProductService.Models;
using ProductService.Strategies;

namespace ProductService.Services;

/// <summary>
/// ProductService demonstrates the Open/Closed Principle:
/// - CLOSED for modification: We don't modify this class when adding new discount types
/// - OPEN for extension: We can add new discount strategies by implementing IDiscountStrategy
/// </summary>
public class ProductService : IProductService
{
    private readonly List<Product> _products;
    private readonly List<IDiscountStrategy> _discountStrategies;
    
    public ProductService()
    {
        // Initialize sample products
        _products = new List<Product>
        {
            new Product(1, "Laptop", 999.99m, "Electronics") { IsOnSale = true },
            new Product(2, "Book", 19.99m, "Books"),
            new Product(3, "Smartphone", 699.99m, "Electronics") { IsOnSale = true },
            new Product(4, "Coffee Mug", 12.99m, "Kitchen"),
            new Product(5, "Headphones", 89.99m, "Electronics"),
            new Product(6, "T-Shirt", 24.99m, "Clothing"),
            new Product(7, "Gaming Console", 499.99m, "Electronics") { IsOnSale = true },
            new Product(8, "Notebook", 8.99m, "Office"),
            new Product(9, "Bluetooth Speaker", 79.99m, "Electronics"),
            new Product(10, "Water Bottle", 15.99m, "Sports")
        };
        
        // Initialize discount strategies
        // This is where we demonstrate OCP - we can add new strategies without modifying this class
        _discountStrategies = new List<IDiscountStrategy>
        {
            new NoDiscountStrategy(),
            new SaleDiscountStrategy(0.15m), // 15% off for sale items
            new CategoryDiscountStrategy("Electronics", 0.10m), // 10% off electronics
            new NewProductDiscountStrategy(30, 0.05m), // 5% off new products (30 days)
            new BulkPurchaseDiscountStrategy(3, 0.20m) // 20% off for bulk purchases (expensive items)
        };
    }
    
    public IEnumerable<Product> GetAllProducts()
    {
        return _products;
    }
    
    public Product? GetProductById(int id)
    {
        return _products.FirstOrDefault(p => p.Id == id);
    }
    
    /// <summary>
    /// This method demonstrates the Open/Closed Principle.
    /// It doesn't need to be modified when we add new discount strategies.
    /// The logic for calculating discounts is delegated to the strategy implementations.
    /// </summary>
    public decimal CalculateFinalPrice(Product product)
    {
        // Find the best applicable discount strategy
        var applicableStrategies = _discountStrategies
            .Where(strategy => strategy.IsApplicable(product))
            .ToList();
        
        if (!applicableStrategies.Any())
        {
            return product.Price; // No discount applied
        }
        
        // Apply the highest discount (you could implement different logic here)
        var maxDiscount = applicableStrategies.Max(strategy => strategy.CalculateDiscount(product));
        var finalPrice = product.Price - maxDiscount;
        
        return Math.Max(finalPrice, 0); // Ensure price doesn't go negative
    }
    
    public IEnumerable<IDiscountStrategy> GetAvailableDiscounts()
    {
        return _discountStrategies;
    }
    
    /// <summary>
    /// Helper method to show which discount strategy is applied to a product
    /// </summary>
    public (IDiscountStrategy Strategy, decimal Discount) GetAppliedDiscount(Product product)
    {
        var applicableStrategies = _discountStrategies
            .Where(strategy => strategy.IsApplicable(product))
            .ToList();
        
        if (!applicableStrategies.Any())
        {
            return (new NoDiscountStrategy(), 0);
        }
        
        var bestStrategy = applicableStrategies
            .OrderByDescending(strategy => strategy.CalculateDiscount(product))
            .First();
        
        return (bestStrategy, bestStrategy.CalculateDiscount(product));
    }
} 