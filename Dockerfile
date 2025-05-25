# Use the official .NET 9 image as a build environment
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Add a non-root user for security
RUN adduser --disabled-password --gecos '' appuser

# Copy contracts and service projects for better layer caching
COPY src/Play.Catalog.Contracts ./Play.Catalog.Contracts
COPY src/Play.Catalog.Service ./Play.Catalog.Service

WORKDIR /src/Play.Catalog.Service
RUN --mount=type=secret,id=GH_OWNER,dst=/GH_OWNER --mount=type=secret,id=GH_PAT,dst=/GH_PAT \
    dotnet nuget add source --username USERNAME --password `cat /GH_PAT` --store-password-in-clear-text --name github "https://nuget.pkg.github.com/`cat /GH_OWNER`/index.json"
RUN dotnet restore
RUN dotnet publish -c Release -o /app

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Add the same non-root user in runtime image
RUN adduser --disabled-password --gecos '' -u 5678 appuser
COPY --from=build /app ./
RUN chown -R appuser /app
ENV ASPNETCORE_URLS=http://+:5000
EXPOSE 5000
USER appuser
ENTRYPOINT ["dotnet", "Play.Catalog.Service.dll"]
