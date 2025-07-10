using ProductService.Services;
using ProductService.Models;

namespace ProductService;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== Open/Closed Principle Demonstration ===\n");
        
        var productService = new Services.ProductService();
        
        // Display all available discount strategies
        Console.WriteLine("Available Discount Strategies:");
        Console.WriteLine("===============================");
        foreach (var strategy in productService.GetAvailableDiscounts())
        {
            Console.WriteLine($"- {strategy.Name}");
        }
        
        Console.WriteLine("\n" + new string('=', 60) + "\n");
        
        // Display all products with their pricing information
        Console.WriteLine("Product Pricing Analysis:");
        Console.WriteLine("========================");
        
        var products = productService.GetAllProducts();
        foreach (var product in products)
        {
            var originalPrice = product.Price;
            var finalPrice = productService.CalculateFinalPrice(product);
            var (appliedStrategy, discount) = productService.GetAppliedDiscount(product);
            
            Console.WriteLine($"\nProduct: {product.Name}");
            Console.WriteLine($"  Category: {product.Category}");
            Console.WriteLine($"  Original Price: ${originalPrice:F2}");
            Console.WriteLine($"  Applied Strategy: {appliedStrategy.Name}");
            Console.WriteLine($"  Discount Amount: ${discount:F2}");
            Console.WriteLine($"  Final Price: ${finalPrice:F2}");
            Console.WriteLine($"  On Sale: {(product.IsOnSale ? "Yes" : "No")}");
            Console.WriteLine($"  Days Since Creation: {(DateTime.Now - product.CreatedDate).Days}");
        }
        
        Console.WriteLine("\n" + new string('=', 60) + "\n");
        
        // Demonstrate how easy it is to add new discount strategies
        Console.WriteLine("Open/Closed Principle Benefits:");
        Console.WriteLine("===============================");
        Console.WriteLine("1. The ProductService class is CLOSED for modification");
        Console.WriteLine("   - We don't need to change the CalculateFinalPrice method");
        Console.WriteLine("   - We don't need to modify the existing discount logic");
        Console.WriteLine();
        Console.WriteLine("2. The ProductService is OPEN for extension");
        Console.WriteLine("   - We can add new discount strategies by implementing IDiscountStrategy");
        Console.WriteLine("   - New strategies automatically work with existing code");
        Console.WriteLine("   - No risk of breaking existing functionality");
        Console.WriteLine();
        Console.WriteLine("3. Example: To add a 'Bulk Purchase Discount', we would:");
        Console.WriteLine("   - Create a new BulkPurchaseDiscountStrategy class");
        Console.WriteLine("   - Implement IDiscountStrategy interface");
        Console.WriteLine("   - Add it to the strategies list in ProductService constructor");
        Console.WriteLine("   - No other code changes needed!");
        
        Console.WriteLine("\n" + new string('=', 60) + "\n");
        
        // Show specific examples of how different strategies apply
        Console.WriteLine("Strategy Application Examples:");
        Console.WriteLine("==============================");
        
        var laptop = products.First(p => p.Name == "Laptop");
        var book = products.First(p => p.Name == "Book");
        var headphones = products.First(p => p.Name == "Headphones");
        
        Console.WriteLine($"\n1. {laptop.Name} (Electronics + On Sale):");
        Console.WriteLine($"   - Original: ${laptop.Price:F2}");
        Console.WriteLine($"   - Final: ${productService.CalculateFinalPrice(laptop):F2}");
        Console.WriteLine($"   - Strategy: {productService.GetAppliedDiscount(laptop).Strategy.Name}");
        
        Console.WriteLine($"\n2. {book.Name} (Books category):");
        Console.WriteLine($"   - Original: ${book.Price:F2}");
        Console.WriteLine($"   - Final: ${productService.CalculateFinalPrice(book):F2}");
        Console.WriteLine($"   - Strategy: {productService.GetAppliedDiscount(book).Strategy.Name}");
        
        Console.WriteLine($"\n3. {headphones.Name} (Electronics only):");
        Console.WriteLine($"   - Original: ${headphones.Price:F2}");
        Console.WriteLine($"   - Final: ${productService.CalculateFinalPrice(headphones):F2}");
        Console.WriteLine($"   - Strategy: {productService.GetAppliedDiscount(headphones).Strategy.Name}");
        
        Console.WriteLine("\n" + new string('=', 60));
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
} 