namespace DependencyInjectionDemo.Services
{
    public class TransientService : ITransientService
    {
        private readonly string _id;

        public TransientService()
        {
            _id = Guid.NewGuid().ToString();
            Console.WriteLine($"TransientService created with ID: {_id}");
        }

        public string GetId()
        {
            return _id;
        }

        public string GetMessage()
        {
            return $"Transient Service - ID: {_id} - Created new instance every time";
        }
    }
} 