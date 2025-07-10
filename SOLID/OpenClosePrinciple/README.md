# Open/Closed Principle (OCP) Demonstration

This project demonstrates the **Open/Closed Principle** from SOLID principles using a .NET Core ProductService with discount strategies.

## What is the Open/Closed Principle?

The Open/Closed Principle states that:
> **Software entities (classes, modules, functions) should be open for extension but closed for modification.**

- **Open for Extension**: You should be able to add new functionality without changing existing code
- **Closed for Modification**: Existing code should not be modified when adding new features

## How This Example Demonstrates OCP

### The Problem (Without OCP)
In a traditional approach, adding new discount types would require modifying the ProductService class:

```csharp
// BAD: This violates OCP - we need to modify existing code
public class ProductService
{
    public decimal CalculateDiscount(Product product)
    {
        if (product.IsOnSale)
            return product.Price * 0.15m;
        else if (product.Category == "Electronics")
            return product.Price * 0.10m;
        else if (product.CreatedDate > DateTime.Now.AddDays(-30))
            return product.Price * 0.05m;
        // Adding new discount types requires modifying this method!
        return 0;
    }
}
```

### The Solution (With OCP)
Our implementation uses the **Strategy Pattern** to make the system open for extension:

```csharp
// GOOD: This follows OCP - we can add new strategies without modifying existing code
public interface IDiscountStrategy
{
    decimal CalculateDiscount(Product product);
    bool IsApplicable(Product product);
}

public class ProductService
{
    private readonly List<IDiscountStrategy> _discountStrategies;
    
    public decimal CalculateFinalPrice(Product product)
    {
        // This method never needs to change when adding new discount types
        var applicableStrategies = _discountStrategies
            .Where(strategy => strategy.IsApplicable(product));
        // ... rest of the logic
    }
}
```

## Project Structure

```
ProductService/
├── Models/
│   └── Product.cs                    # Product domain model
├── Interfaces/
│   ├── IDiscountStrategy.cs         # Strategy interface (OCP foundation)
│   └── IProductService.cs           # Service interface
├── Strategies/                      # Discount strategy implementations
│   ├── NoDiscountStrategy.cs        # Base strategy (no discount)
│   ├── SaleDiscountStrategy.cs      # Sale items discount
│   ├── CategoryDiscountStrategy.cs  # Category-based discount
│   └── NewProductDiscountStrategy.cs # New products discount
├── Services/
│   └── ProductService.cs            # Main service (closed for modification)
└── Program.cs                       # Demonstration program
```

## Available Discount Strategies

1. **No Discount** - Default strategy for all products
2. **Sale Discount** - 15% off for products marked as on sale
3. **Category Discount** - 10% off for Electronics category
4. **New Product Discount** - 5% off for products created within 30 days

## How to Add a New Discount Strategy

To add a new discount type (e.g., "Bulk Purchase Discount"), you would:

1. **Create a new strategy class** (no modification to existing code):
```csharp
public class BulkPurchaseDiscountStrategy : IDiscountStrategy
{
    public decimal CalculateDiscount(Product product)
    {
        // New discount logic
        return product.Price * 0.20m; // 20% off for bulk purchases
    }
    
    public bool IsApplicable(Product product)
    {
        // New applicability logic
        return product.Quantity > 5; // Apply to bulk purchases
    }
}
```

2. **Register the strategy** (only addition, no modification):
```csharp
// In ProductService constructor, add:
_discountStrategies.Add(new BulkPurchaseDiscountStrategy());
```

3. **That's it!** The new discount automatically works with all existing code.

## Benefits of This Approach

✅ **No Risk**: Adding new features doesn't risk breaking existing functionality  
✅ **Maintainable**: Each discount strategy is isolated and easy to test  
✅ **Extensible**: New discount types can be added without touching existing code  
✅ **Testable**: Each strategy can be unit tested independently  
✅ **Flexible**: Strategies can be easily swapped or combined  

## Running the Example

```bash
cd ProductService
dotnet run
```

The program will demonstrate:
- All available discount strategies
- How each product gets priced with different strategies
- Examples of how the Open/Closed Principle works in practice

## Key Takeaways

1. **Use Interfaces**: Define contracts that can be implemented by new classes
2. **Delegate Behavior**: Let strategy objects handle specific logic
3. **Composition over Inheritance**: Use composition to add new behaviors
4. **Single Responsibility**: Each strategy has one job
5. **Dependency Inversion**: Depend on abstractions, not concretions

This example shows how following the Open/Closed Principle leads to more maintainable, extensible, and robust code. 