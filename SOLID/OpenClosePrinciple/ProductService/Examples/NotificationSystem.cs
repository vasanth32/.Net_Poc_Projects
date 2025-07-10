using System.Text.Json;

namespace ProductService.Examples;

// Real-world example: Notification System
// This demonstrates how OCP is used in actual projects

public class NotificationMessage
{
    public string To { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public Dictionary<string, object> Metadata { get; set; } = new();
}

public interface INotificationService
{
    string ServiceName { get; }
    Task<bool> SendAsync(NotificationMessage message);
    bool CanSendTo(string recipient);
}

// Email notification implementation
public class EmailNotificationService : INotificationService
{
    public string ServiceName => "Email";
    
    public async Task<bool> SendAsync(NotificationMessage message)
    {
        // Simulate email sending
        await Task.Delay(100);
        Console.WriteLine($"📧 Email sent to {message.To}: {message.Subject}");
        return true;
    }
    
    public bool CanSendTo(string recipient)
    {
        return recipient.Contains("@");
    }
}

// SMS notification implementation
public class SMSNotificationService : INotificationService
{
    public string ServiceName => "SMS";
    
    public async Task<bool> SendAsync(NotificationMessage message)
    {
        // Simulate SMS sending
        await Task.Delay(50);
        Console.WriteLine($"📱 SMS sent to {message.To}: {message.Body}");
        return true;
    }
    
    public bool CanSendTo(string recipient)
    {
        return recipient.All(char.IsDigit) && recipient.Length >= 10;
    }
}

// Push notification implementation
public class PushNotificationService : INotificationService
{
    public string ServiceName => "Push";
    
    public async Task<bool> SendAsync(NotificationMessage message)
    {
        // Simulate push notification
        await Task.Delay(75);
        Console.WriteLine($"🔔 Push notification sent to {message.To}: {message.Subject}");
        return true;
    }
    
    public bool CanSendTo(string recipient)
    {
        return recipient.StartsWith("device_");
    }
}

// Slack notification implementation
public class SlackNotificationService : INotificationService
{
    public string ServiceName => "Slack";
    
    public async Task<bool> SendAsync(NotificationMessage message)
    {
        // Simulate Slack message
        await Task.Delay(60);
        Console.WriteLine($"💬 Slack message sent to {message.To}: {message.Body}");
        return true;
    }
    
    public bool CanSendTo(string recipient)
    {
        return recipient.StartsWith("#") || recipient.StartsWith("@");
    }
}

// Main notification orchestrator (closed for modification, open for extension)
public class NotificationOrchestrator
{
    private readonly List<INotificationService> _services;
    
    public NotificationOrchestrator()
    {
        // Initialize with available services
        // Adding new notification services doesn't require modifying this class
        _services = new List<INotificationService>
        {
            new EmailNotificationService(),
            new SMSNotificationService(),
            new PushNotificationService(),
            new SlackNotificationService()
        };
    }
    
    public async Task<bool> SendNotificationAsync(NotificationMessage message)
    {
        var applicableServices = _services.Where(s => s.CanSendTo(message.To)).ToList();
        
        if (!applicableServices.Any())
        {
            Console.WriteLine($"❌ No notification service can handle recipient: {message.To}");
            return false;
        }
        
        var results = new List<bool>();
        foreach (var service in applicableServices)
        {
            try
            {
                var result = await service.SendAsync(message);
                results.Add(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error sending via {service.ServiceName}: {ex.Message}");
                results.Add(false);
            }
        }
        
        return results.Any(r => r);
    }
    
    public IEnumerable<string> GetAvailableServices()
    {
        return _services.Select(s => s.ServiceName);
    }
    
    // This method demonstrates how easy it is to add new functionality
    // without modifying existing code
    public async Task<bool> SendToAllChannelsAsync(NotificationMessage message)
    {
        var tasks = _services.Select(service => service.SendAsync(message));
        var results = await Task.WhenAll(tasks);
        return results.Any(r => r);
    }
} 