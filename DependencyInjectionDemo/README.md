# Dependency Injection Demo

This project demonstrates the three types of dependency injection in .NET Core:

## 1. Singleton
- **Lifetime**: One instance for the entire application lifetime
- **Use Case**: Services that are stateless and expensive to create (e.g., configuration, logging)
- **Registration**: `AddSingleton<TInterface, TImplementation>()`

## 2. Scoped
- **Lifetime**: One instance per HTTP request (or scope)
- **Use Case**: Services that need to maintain state within a request (e.g., database context)
- **Registration**: `AddScoped<TInterface, TImplementation>()`

## 3. Transient
- **Lifetime**: New instance every time it's requested
- **Use Case**: Lightweight, stateless services
- **Registration**: `AddTransient<TInterface, TImplementation>()`

## How to Run

1. Navigate to the project directory
2. Run the application:
   ```bash
   dotnet run
   ```
3. Open your browser and go to: `http://localhost:5000/api/demo`

## Testing the Demo

### Basic Demo
- **Endpoint**: `GET /api/demo`
- **Purpose**: Shows the IDs of all three service types in a single request

### Multiple Calls Test
- **Endpoint**: `GET /api/demo/multiple`
- **Purpose**: Shows how services behave when called multiple times within the same request

### Compare Services (Key Difference)
- **Endpoint**: `GET /api/demo/compare`
- **Purpose**: **This is the most important endpoint!** It injects additional instances of Scoped and Transient services to clearly show the difference:
  - **Scoped**: Same instance within the same request
  - **Transient**: Different instances every time

### Request Comparison
- **Endpoint**: `GET /api/demo/request-comparison`
- **Purpose**: Make multiple requests to this endpoint to see how services behave across different requests

## Expected Behavior

1. **Singleton**: Same ID across all requests and calls
2. **Scoped**: Same ID within the same HTTP request, different ID for different requests
3. **Transient**: Different ID every time it's requested, even within the same request

## Key Difference Between Scoped and Transient

The main difference becomes clear when you inject multiple instances of the same service within the same request:

- **Scoped**: All instances within the same request share the same ID
- **Transient**: Each instance gets a different ID, even within the same request

## Console Output
Watch the console output to see when each service is created:
- SingletonService: Created once when the application starts
- ScopedService: Created once per HTTP request
- TransientService: Created every time it's injected 