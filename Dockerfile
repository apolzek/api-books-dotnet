# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /source

COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o publish_output

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
ENV ASPNETCORE_ENVIRONMENT="Production"
EXPOSE 4000
COPY --from=build source/publish_output/ /app
ENTRYPOINT ["dotnet", "BooksApi.dll"]
