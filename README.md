# api-books-dotnet5

Web API with .NET and MongoDB :heavy_check_mark: .NET 5.0

> 5.0.402

*Create a web API that performs Create, Read, Update, and Delete (CRUD) operations on a MongoDB NoSQL database.*

## Prerequisites

- [.NET 5.0](https://dotnet.microsoft.com/download) or later
- MongoDB
- Docker Engine 20.10.10
- docker-compose version 1.29.2

## Run development mode

*rest api*:
```
cd api-books-dotnet5/
dotnet restore
dotnet run
```
> browser: http://localhost:4000/swagger

*mongodb*:

```
docker run --rm --name mongodb -p 27017:27017 mongo:latest
# or
sudo systemctl start mongod
```

*test* :
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
docker build -t api-books .
```
> Note: api-books can be replaced

## Test Docker Image

```
docker run -p 4000:4000 -d api-books:latest
```
> Note: The image must be created in advance

## docker-compose

```
docker-compose up -d
```
> Note: Change 'localhost' to 'mongo-example' in file appsettings.json. Build and generate a new docker image.

## Details

### Mongo object example 

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

### Change port

Edit *Program.cs* file

```
WebHost.CreateDefaultBuilder(args)
    .UseStartup<Startup>()
    .UseUrls(urls: "http://0.0.0.0:4000")
    .Build();
```

### Insert manually

mongo cli

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
> Note: install jq before

### Test the web API

  - Navigate to `http://localhost:<port>/api/Books`
  - Example: `http://localhost:4000/api/Books`

### swagger(open on brownser)

  - Navigate to `http://localhost:<port>/swagger/index.html`
  - Example: `http://localhost:4000/swagger/index.html`

### HealthCheck
  - Access http://localhost:4000/health
