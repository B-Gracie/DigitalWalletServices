FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

# Copy the solution file
COPY E-Wallet.sln .

# Copy and restore the DAL project
COPY Wallet.DAL/Wallet.DAL.csproj ./Wallet.DAL/
RUN dotnet restore Wallet.DAL/Wallet.DAL.csproj

# Copy and restore the Services project
COPY Wallet.Services/Wallet.Services.csproj ./Wallet.Services/
RUN dotnet restore Wallet.Services/Wallet.Services.csproj

# Copy and restore the Web project
COPY Wallet.Web/Wallet.Web.csproj ./Wallet.Web/
RUN dotnet restore Wallet.Web/Wallet.Web.csproj

# Copy the entire solution directory
COPY . .

# Publish the Web project
RUN dotnet publish Wallet.Web/Wallet.Web.csproj -c Release -o out

# Build the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app
COPY --from=build-env /app/out ./

# Set the entry point
ENTRYPOINT ["dotnet", "Wallet.Web.dll"]
