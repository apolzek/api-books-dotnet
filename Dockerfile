FROM mcr.microsoft.com/dotnet/aspnet:5.0

WORKDIR /app
EXPOSE 4000
COPY publish_output .
ENTRYPOINT ["dotnet", "BooksApi.dll"]
