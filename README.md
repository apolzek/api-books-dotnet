# api-books-aspnet-core

Web API with ASP.NET  Core and MongoDB :heavy_check_mark: .NET Core SDK 5

*Create a web API that performs Create, Read, Update, and Delete (CRUD) operations on a MongoDB NoSQL database.*

## Prerequisites

- .NET Core SDK 5.0 or later
- Visual Studio Code 
- MongoDB(docker or service)
- Docker Engine 20.10.5
- docker-compose version 1.28.5

## Run source code

```
dotnet restore
dotnet run
```
### Build / Create a Docker Image / Test

```
dotnet publish -c Release -o publish_output
docker build -t apibooks .
```

```
docker run -p 4000:4000 -d apibooks:latest
```

or

```
docker-compose up -d
```

> OBS: 'apibooks' Can be changed to a name of your choice

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

```
mongo
use BookstoreDb
db.Books.insertMany([{'BookName':'Design Patterns','Price':54.93,'Category':'Computers','Author':'Ralph Johnson'}, {'BookName':'Clean Code','Price':43.15,'Category':'Computers','Author':'Robert C. Martin'}])
```

### Fake requests

```
chmod +x fake-requests.sh
./fake-requests.sh
```

### Test the web API

  - Navigate to `http://localhost:<port>/api/books`
  - Example: `http://localhost:4000/api/books`

### swagger

  - Navigate to `http://localhost:<port>/swagger/index.html`
  - Example: `http://localhost:4000/swagger/index.html`
