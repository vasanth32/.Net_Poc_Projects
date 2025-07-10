using ProductService.Models;

namespace ProductService.Interfaces;

/// <summary>
/// This interface demonstrates the Open/Closed Principle.
/// The ProductService is CLOSED for modification (we don't change existing discount logic)
/// but OPEN for extension (we can add new discount strategies by implementing this interface)
/// </summary>
public interface IDiscountStrategy
{
    string Name { get; }
    decimal CalculateDiscount(Product product);
    bool IsApplicable(Product product);
} 