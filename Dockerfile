FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy only the project files first so restore is cached unless a .csproj changes
COPY Vector.slnx ./
COPY src/Vector.Domain/Vector.Domain.csproj src/Vector.Domain/
COPY src/Vector.Application/Vector.Application.csproj src/Vector.Application/
COPY src/Vector.Infrastructure/Vector.Infrastructure.csproj src/Vector.Infrastructure/
COPY src/Vector.API/Vector.API.csproj src/Vector.API/
RUN dotnet restore src/Vector.API/Vector.API.csproj

COPY src/ src/
RUN dotnet publish src/Vector.API/Vector.API.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Vector.API.dll"]
