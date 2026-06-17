# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy project files and restore dependencies
COPY ["Backend.csproj", "./"]
RUN dotnet restore "Backend.csproj"

# Copy the rest of the source code and build
COPY . .
RUN dotnet build "Backend.csproj" -c Release -o /app/build

# Publish Stage
FROM build AS publish
RUN dotnet publish "Backend.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Production Stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app/Backend
COPY --from=publish /app/publish .
COPY PdfHtmlTemplates ./PdfHtmlTemplates

# Environment configuration for Playwright
ENV PLAYWRIGHT_BROWSERS_PATH=/app/Backend/.playwright-browsers

# Install system dependencies for Playwright and download Chromium
USER root
RUN dotnet exec --runtimeconfig Backend.runtimeconfig.json Microsoft.Playwright.dll install --with-deps chromium

ENV ASPNETCORE_URLS=http://+:5275
EXPOSE 5275
ENTRYPOINT ["dotnet", "Backend.dll"]
