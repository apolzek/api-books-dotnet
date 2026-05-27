# api-books-dotnet

REST API with .NET and MongoDB

:whale2: **image**: apolzek/api-books:1.1

Create a web API that performs Create, Read, Update, and Delete (CRUD) operations on a MongoDB NoSQL database

- [x] Health Check
- [x] MongoDB Persistence 
- [x] Dockerfile and docker-compose
- [x] Kubernetes manifests
- [x] Swagger

## Prerequisites

- [.NET 9.0](https://dotnet.microsoft.com/download)
- MongoDB
- Docker Engine >= 28.0.4
- Docker Compose >= v2.34.0
- Helm >= v3.17.0

## Getting started

Start MongoDB
```
docker run --rm --name mongodb -d -p 27017:27017 mongo:latest 
```

Run application
```
dotnet restore
export ASPNETCORE_ENVIRONMENT=Development && dotnet run
```
> browser: http://localhost:4000/swagger

Checking
```
curl http://0.0.0.0:4000/health
curl -X GET "http://0.0.0.0:4000/api/Books"
```

## Changelog (security & hardening)

- **Removed** `GET /api/test/ConnectionString` (exposed MongoDB connection string).
- **Changed** `GET /api/test/PrintHostname` and `GET /api/test/IP` — now use `System.Net.Dns` / `NetworkInterface` instead of shelling out to `/bin/bash`.
- **Added** `Book` model validation (`[Required]`, `[StringLength]`, `[Range]`) — invalid POST/PUT now returns `400` with a ProblemDetails body listing each field error.
- **Added** global exception handler (`UseExceptionHandler` + `AddProblemDetails`) — unhandled exceptions now return RFC 9457 ProblemDetails in Production instead of a raw stack trace.
- **Changed** Dockerfile — runs as non-root `app` user (uid 1000).
- **Changed** `.dockerignore` — now excludes `bin/`, `obj/`, `.git/`, `.github/`, `tests/`, `helm/`, `k8s/`, IDE/editor folders.
- **Changed** `k8s/api-books.yml` — added liveness/readiness probes on `/health`, resource requests/limits, `runAsNonRoot` pod securityContext, drop-all capabilities.
- **Changed** `k8s/mongo.yml` — bumped `mongo:4.0.8` → `mongo:7.0`, added TCP probes and resource limits.
- **Changed** `docker-compose.yaml` — pinned `mongo:7.0` (was floating `mongo:latest`).
- **Changed** `helm/values.yaml` — `podSecurityContext`, `securityContext`, and `resources` now populated (were `{}`).
- **Changed** `.github/workflows/dotnet.yml` — upgraded `actions/checkout@v2` → `@v4`, `actions/setup-dotnet@v1` → `@v4`, enabled `dotnet build` and `dotnet test` steps.
- **Changed** `BooksController` — annotated every action with `[ProducesResponseType]` so Swagger documents 200/201/204/400/404.
- **Changed** `BooksApi.csproj` — opted into `<Nullable>warnings</Nullable>`.

## Tests

Integration tests for the API live in `tests/BooksApi.Tests` (xUnit + `Microsoft.AspNetCore.Mvc.Testing`). They spin up the app in-memory with `WebApplicationFactory` and exercise the `TestController` endpoints plus the `/health` check — no MongoDB required.

Run with the local .NET SDK:
```
dotnet test tests/BooksApi.Tests/BooksApi.Tests.csproj
```

Or, without installing .NET locally, run inside the official SDK container:
```
docker run --rm -v "$PWD":/src -w /src mcr.microsoft.com/dotnet/sdk:9.0 \
  dotnet test tests/BooksApi.Tests/BooksApi.Tests.csproj
```

## Create/Publish a Docker Image

```
docker build -t <user>/api-books<tagname> .
docker login
docker push <user>/api-books<tagname>
```

## Docker Compose

```
docker compose up -d
```

## kubernetes

Create a local cluster with kind
```
kind create cluster --name k8s-local-with-kind --config kind.yaml
kind get clusters
```

### Deploy Yaml

Deploy api-books and mongo-example
```
kubectl apply -f ./k8s
```

or
```
kubectl apply -f https://raw.githubusercontent.com/apolzek/api-books-dotnet/main/k8s/api-books.yml
kubectl apply -f https://raw.githubusercontent.com/apolzek/api-books-dotnet/main/k8s/mongo.yml
```

### Deploy with Helm

```
# deploy app
helm install api-books-dotnet helm/

# deploy database
helm repo add bitnami https://charts.bitnami.com/bitnami
helm install mongo-example bitnami/mongodb --set fullnameOverride=mongo-example --set auth.enabled=false
```

### Port-forward

```
kubectl port-forward svc/api-books-dotnet 4000:4000
```

## API Details

### Mongo object example(Books)

```javascript
{
  "_id" : ObjectId("5bfd996f7b8e48dc15ff215d"),
  "Name" : "Design Patterns",
  "Price" : 54.93,
  "Category" : "Computers",
  "Author" : "Ralph Johnson"
}
{
  "_id" : ObjectId("5bfd996f7b8e48dc15ff215e"),
  "Name" : "Clean Code",
  "Price" : 43.15,
  "Category" : "Computers",
  "Author" : "Robert C. Martin"
}
```

### Change port

Edit *Program.cs* file
```
WebHost.CreateDefaultBuilder(args)
    .UseStartup<Startup>()
    .UseUrls(urls: "http://0.0.0.0:4000")
    .Build();
```
> OBS: Impacts Docker image. change port in Dockerfile

### Insert manually(mongo)

MongoDB cli
```
docker exec -it $(docker ps | grep mongo | awk '{print $1}')  bash
mongosh
use BookstoreDb
db.Books.insertMany([{'BookName':'Design Patterns','Price':54.93,'Category':'Computers','Author':'Ralph Johnson'}, {'BookName':'Clean Code','Price':43.15,'Category':'Computers','Author':'Robert C. Martin'}])
```

### Swagger(open on brownser)

  - Navigate to `http://localhost:<port>/swagger/index.html`
  - Example: `http://localhost:4000/swagger/index.html`

### HealthCheck

  - Access http://localhost:4000/health
