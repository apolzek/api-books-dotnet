# api-books-dotnet

REST API with .NET and MongoDB

:whale2: **image**: apolzek/api-books:1.0

Create a web API that performs Create, Read, Update, and Delete (CRUD) operations on a MongoDB NoSQL database

- [x] Health Check
- [x] MongoDB Persistence 
- [x] Docker/ Docker Compose
- [x] Kubernetes manifests
- [x] Swagger

## Prerequisites

- [.NET 6.0](https://dotnet.microsoft.com/download)
- MongoDB
- Docker Engine >= 20.10.17 
- Docker Compose >= v2.6.0
- Helm >= v3.8.0

## Getting started

Start MongoDB:
```
docker run --rm --name mongodb -p 27017:27017 mongo:latest
```
or
```
sudo systemctl start mongod
```

Run application:
```
dotnet restore
export ASPNETCORE_ENVIRONMENT=Development && dotnet run
```
> browser: http://localhost:4000/swagger

Checking:
```
curl http://0.0.0.0:4000/health
curl -X GET "http://0.0.0.0:4000/api/Books" -H  "accept: text/plain"
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
docker compose down
```

## kubernetes

### Create a local cluster with kind

```
kind create cluster --name demo-api-books --config kind.yaml
kind get clusters
kind delete clusters demo-api-books
```

### Traditional Yaml

*Deploy api-books and mongo-example*

```bash
kubectl apply -f ./k8s
kubectl delete -f ./k8s
```
or
```
kubectl apply -f https://raw.githubusercontent.com/apolzek/api-books-dotnet/main/k8s/apibooks.yml
kubectl apply -f https://raw.githubusercontent.com/apolzek/api-books-dotnet/main/k8s/mongo.yml
```

###  Helm

```bash
helm install api-books-dotnet helm/
helm install mongo-example bitnami/mongodb --set fullnameOverride=mongo-example --set auth.enabled=false

helm delete api-books-dotnet
helm delete mongo-example
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
> /api/Books


### MongoDB settings

Add the following database configuration values to *appsettings.json*:

```javascript
{
  "BookstoreDatabaseSettings": {
  "BooksCollectionName": "Books",
  "ConnectionString": "mongodb://localhost:27017",
  "DatabaseName": "BookstoreDb"
},
```
> OBS: Replace localhost with your mongodb address

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

*mongo cli*

```
docker exec -it <CONTAINER_ID> bash
mongo
use BookstoreDb
db.Books.insertMany([{'BookName':'Design Patterns','Price':54.93,'Category':'Computers','Author':'Ralph Johnson'}, {'BookName':'Clean Code','Price':43.15,'Category':'Computers','Author':'Robert C. Martin'}])
```

### swagger(open on brownser)

  - Navigate to `http://localhost:<port>/swagger/index.html`
  - Example: `http://localhost:4000/swagger/index.html`

### HealthCheck

  - Access http://localhost:4000/health
