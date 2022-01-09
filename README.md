# api-books-dotnet

REST API with .NET and MongoDB :heavy_check_mark: CRUD

> dotnet --version => 6.0.101

:whale2: **image**: apolzek/api-books:v1.7

*Create a web API that performs Create, Read, Update, and Delete (CRUD) operations on a MongoDB NoSQL database.*

## Prerequisites

- [.NET 6.0](https://dotnet.microsoft.com/download)
- MongoDB
- Docker Engine 20.10.10 >=
- docker-compose version 1.29.2 >=

## Run development mode


*1. run api*:

```
cd api-books-dotnet/
dotnet restore
export ASPNETCORE_ENVIRONMENT=Development && dotnet run
```
> browser: http://localhost:4000/swagger

*2. mongodb*:

```
docker run --rm --name mongodb -p 27017:27017 mongo:latest
# or
sudo systemctl start mongod
```

*3. validation* :

```
curl http://0.0.0.0:4000/health

# test connectivity with mongodb
curl -X GET "http://0.0.0.0:4000/api/Books" -H  "accept: text/plain"
```

## Build 

```
dotnet publish -c Release -o publish_output
```

## Create a Docker Image

```
docker build -t <user>/api-books .
```
> Note: api-books can be replaced

## Test Docker Image

```
docker run -p 4000:4000 -d api-books:v1.7
```
> Note: The image must be created in advance

## docker-compose

```
docker-compose up -d
```

## kubernetes

### Traditional(Yaml)

*Deploy api-books and mongo-example*

```bash
cd k8s/
kubectl apply -f .
```
###  Helm

```bash
cd helm/

helm install api-books-dotnet .

# Default: auth.enabled=true
helm install mongo-example bitnami/mongodb --set fullnameOverride=mongo-example --set auth.enabled=false
```

> kubectl v1.22.3

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

### Random tests 

> http://0.0.0.0:4000/api/test/<..>

It has a controller with several tests for study purposes only. Ex:

```
curl -X GET "http://0.0.0.0:4000/api/test/GenerateException" -H  "accept: text/plain"
```

> Force return status code 500 

### Change port

Edit *Program.cs* file

```
WebHost.CreateDefaultBuilder(args)
    .UseStartup<Startup>()
    .UseUrls(urls: "http://0.0.0.0:4000")
    .Build();
```

> OBS: Impacts Docker image. change port in Dockerfile

### Insert manually

*mongo cli*

```
docker exec -it <CONTAINER_ID> bash
mongo
use BookstoreDb
db.Books.insertMany([{'BookName':'Design Patterns','Price':54.93,'Category':'Computers','Author':'Ralph Johnson'}, {'BookName':'Clean Code','Price':43.15,'Category':'Computers','Author':'Robert C. Martin'}])
```

### Fake requests

```
chmod +x fake-requests.sh
./fake-requests.sh
```
> Note: Install jq before

### Test the web API

  - Navigate to `http://localhost:<port>/api/Books`
  - Example: `http://localhost:4000/api/Books`

### swagger(open on brownser)

  - Navigate to `http://localhost:<port>/swagger/index.html`
  - Example: `http://localhost:4000/swagger/index.html`

### HealthCheck
  - Access http://localhost:4000/health

### scanapi

Install:

```
cd scanapi
virtualenv venv
source venv/bin/activate
python -m pip install --upgrade pip
pip install scanapi
```

Run:

```
scanapi run
```