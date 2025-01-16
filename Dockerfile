# Use the official .NET 8 SDK image to build the app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 81

# Use the official .NET 8 SDK image for building the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Candidate.API/Candidate.API.csproj", "Candidate.API/"]
COPY ["Candidate.Application/Candidate.Application.csproj", "Candidate.Application/"]
COPY ["Candidate.Domain/Candidate.Domain.csproj", "Candidate.Domain/"]
COPY ["Candidate.Infrastructure/Candidate.Infrastructure.csproj", "Candidate.Infrastructure/"]
RUN dotnet restore "Candidate.API/Candidate.API.csproj"

# Copy the rest of the application code
COPY . .

# Copy wait-for-it.sh script
COPY wait-for-it.sh /usr/local/bin/wait-for-it.sh
RUN chmod +x /usr/local/bin/wait-for-it.sh
# Build the application
WORKDIR /src/Candidate.API
RUN dotnet build "Candidate.API.csproj" -c Release -o /app/build

# Publish the application
RUN dotnet publish "Candidate.API.csproj" -c Release -o /app/publish

# Final image for running the application
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Candidate.API.dll"]

