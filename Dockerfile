# Build stage
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

# Copy the solution file
COPY E-Wallet.sln .

# Copy and restore the projects
COPY Wallet.DAL/Wallet.DAL.csproj ./Wallet.DAL/
COPY Wallet.Services/Wallet.Services.csproj ./Wallet.Services/
COPY Wallet.Web/Wallet.Web.csproj ./Wallet.Web/
RUN dotnet restore

# Copy the entire solution directory
COPY . .

# Publish the Web project
RUN dotnet publish Wallet.Web/Wallet.Web.csproj -c Release -o /app/out

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app
COPY --from=build-env /app/out .

# Set the entry point
CMD ["dotnet", "Wallet.Web.exe", "--cpus", "100"]
