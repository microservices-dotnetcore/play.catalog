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

## Build the docker image
```sh
$env:GH_OWNER="microservices-dotnetcore"
$env:GH_PAT="[PAT HERE]"
$version="1.0.0"
docker build --secret id=GH_OWNER --secret id=GH_PAT -t play.catalog:$version .
```

## Run the docker image
```sh
$version="1.0.0"
docker run -it --rm -p 5000:5000 --name catalog `
  -e MongoDbSettings__Host=mongo `
  -e RabbitMQSettings__Host=rabbitmq `
  --network playinfra_default `
  play.catalog:$version
```

## Requirements
- .NET 9 or later

---
