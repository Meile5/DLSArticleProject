# Use the .NET runtime for the final image
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

# Use the SDK image for building the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the csproj and restore dependencies
COPY ["ArticleService.csproj", "./"]
RUN dotnet restore "ArticleService.csproj"

# Copy all other files and build
COPY . .
RUN dotnet build "ArticleService.csproj" -c Release -o /app/build

# Publish the app
FROM build AS publish
RUN dotnet publish "ArticleService.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ArticleService.dll"]
