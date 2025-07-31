# Use the official .NET 9.0 SDK image as the build image
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

# Set the working directory
WORKDIR /src

# Copy the project file and restore dependencies
COPY ["KubernateBase.csproj", "./"]
RUN dotnet restore "KubernateBase.csproj"

# Copy the rest of the source code
COPY . .

# Build the application
RUN dotnet build "KubernateBase.csproj" -c Release -o /app/build

# Publish the application
RUN dotnet publish "KubernateBase.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Build the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final

# Set the working directory
WORKDIR /app

# Copy the published application from the build stage
COPY --from=build /app/publish .

# Expose port 80 and 443
EXPOSE 80
EXPOSE 443

# Set the entry point
ENTRYPOINT ["dotnet", "KubernateBase.dll"] 