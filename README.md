# api-books-aspnet-core

Web API with ASP.NET  Core and MongoDB :heavy_check_mark: .NET Core SDK 5

*Create a web API that performs Create, Read, Update, and Delete (CRUD) operations on a MongoDB NoSQL database.*

## Prerequisites

- .NET Core SDK 5.0.103 or later
- Visual Studio Code 
- MongoDB(docker or service)

## Mongo object example 

  ```json
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

## MongoDB settings

Add the following database configuration values to *appsettings.json*:

  ```javascript
    {
      "BookstoreDatabaseSettings": {
        "BooksCollectionName": "Books",
        "ConnectionString": "mongodb://localhost:27017",
        "DatabaseName": "BookstoreDb"
      },
  ```

### Insert manually
 
  ```
    db.Books.insertMany([{'Name':'Design Patterns','Price':54.93,'Category':'Computers','Author':'Ralph Johnson'}, {'Name':'Clean Code','Price':43.15,'Category':'Computers','Author':'Robert C. Martin'}])
  ```

## Test the web API

  - Navigate to `http://localhost:<port>/api/books`