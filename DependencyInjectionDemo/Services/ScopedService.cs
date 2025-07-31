namespace DependencyInjectionDemo.Services
{
    public class ScopedService : IScopedService
    {
        private readonly string _id;

        public ScopedService()
        {
            _id = Guid.NewGuid().ToString();
            Console.WriteLine($"ScopedService created with ID: {_id}");
        }

        public string GetId()
        {
            return _id;
        }

        public string GetMessage()
        {
            return $"Scoped Service - ID: {_id} - Created once per HTTP request";
        }
    }
} 