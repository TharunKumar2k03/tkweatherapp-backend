# Use the official .NET SDK image with the desired version
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory
WORKDIR /app

# Copy the project files
COPY . .

# Restore the dependencies
RUN dotnet restore

# Build the application
RUN dotnet publish -c Release -o out

# Use the runtime image for the final stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

# Set the working directory
WORKDIR /app

# Copy the build output from the previous stage
COPY --from=build /app/out .

# Expose the port on which the application runs
EXPOSE 80

# Define the entry point for the application
ENTRYPOINT ["dotnet", "ServerWeather.dll"]
