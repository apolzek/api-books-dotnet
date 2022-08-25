# api-books-dotnet

REST API with .NET and MongoDB

:whale2: **image**: apolzek/api-books:1.0

Create a web API that performs Create, Read, Update, and Delete (CRUD) operations on a MongoDB NoSQL database

- [x] Health Check
- [x] MongoDB Persistence 
- [x] Dockerfile and docker-compose
- [x] Kubernetes manifests
- [x] Swagger

## Prerequisites

- [.NET 6.0](https://dotnet.microsoft.com/download)
- MongoDB
- Docker Engine >= 20.10.17 
- Docker Compose >= v2.6.0
- Helm >= v3.8.0

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
kind create cluster --name demo-api-books --config kind.yaml
kind get clusters
```

### Deploy Yaml

Deploy api-books and mongo-example
```
kubectl apply -f ./k8s
```

or
```
kubectl apply -f https://raw.githubusercontent.com/apolzek/api-books-dotnet/main/k8s/apibooks.yml
kubectl apply -f https://raw.githubusercontent.com/apolzek/api-books-dotnet/main/k8s/mongo.yml
```

### Deploy with Helm

```
helm install api-books-dotnet helm/
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
docker exec -it <CONTAINER_ID> bash
mongo
use BookstoreDb
db.Books.insertMany([{'BookName':'Design Patterns','Price':54.93,'Category':'Computers','Author':'Ralph Johnson'}, {'BookName':'Clean Code','Price':43.15,'Category':'Computers','Author':'Robert C. Martin'}])
```

### Swagger(open on brownser)

  - Navigate to `http://localhost:<port>/swagger/index.html`
  - Example: `http://localhost:4000/swagger/index.html`

### HealthCheck

  - Access http://localhost:4000/health
