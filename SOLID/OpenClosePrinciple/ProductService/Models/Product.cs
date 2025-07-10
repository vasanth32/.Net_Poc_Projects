namespace ProductService.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Category { get; set; } = string.Empty;
    public bool IsOnSale { get; set; }
    public DateTime CreatedDate { get; set; }
    
    public Product(int id, string name, decimal price, string category)
    {
        Id = id;
        Name = name;
        Price = price;
        Category = category;
        CreatedDate = DateTime.Now;
    }
} 