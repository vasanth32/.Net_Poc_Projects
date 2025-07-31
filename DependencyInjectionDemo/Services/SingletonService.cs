namespace DependencyInjectionDemo.Services
{
    public class SingletonService : ISingletonService
    {
        private readonly string _id;

        public SingletonService()
        {
            _id = Guid.NewGuid().ToString();
            Console.WriteLine($"SingletonService created with ID: {_id}");
        }

        public string GetId()
        {
            return _id;
        }

        public string GetMessage()
        {
            return $"Singleton Service - ID: {_id} - Created once for the entire application lifetime";
        }
    }
} 