# Play.Catalog

This project contains the Catalog service and related contracts for the GameShop solution.

- `Play.Catalog.Service`: The main Catalog microservice.
- `Play.Catalog.Contracts`: Shared contracts for Catalog events and messages.

## Getting Started

1. Build the solution using your preferred .NET build tool.
2. Run the Catalog service from the `Play.Catalog.Service` project.

## Project Structure
- `src/Play.Catalog.Service/`
- `src/Play.Catalog.Contracts/`

## Create and publish contract package
```sh
$version="1.0.2"
$owner="microservices-dotnetcore"
$packageName="Play.Catalog.Contracts"
$repoName="play.catalog"
$gh_pat="[]"

dotnet pack Play.Catalog\src\Play.Catalog.Contracts\ --configuration Release -p:PackageVersion=$version -p:RepositoryUrl=https://github.com/$owner/$repoName -o Packages

dotnet nuget push Packages\$packageName.$version.nupkg --api-key $gh_pat --source "github"
```

## Requirements
- .NET 6 or later

---
