FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj files first to leverage cache
COPY src/Domain/VisionCare.Domain.csproj ./src/Domain/
COPY src/Application/VisionCare.Application.csproj ./src/Application/
COPY src/Infrastructure/VisionCare.Infrastructure.csproj ./src/Infrastructure/
COPY src/WebAPI/VisionCare.WebAPI.csproj ./src/WebAPI/

RUN dotnet restore ./src/WebAPI/VisionCare.WebAPI.csproj

# Now copy the full source
COPY src ./src

RUN dotnet publish ./src/WebAPI/VisionCare.WebAPI.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "VisionCare.WebAPI.dll"]
