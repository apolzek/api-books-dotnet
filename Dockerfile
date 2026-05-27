# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /source

COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o publish_output

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
ENV ASPNETCORE_ENVIRONMENT="Production"
EXPOSE 4000
COPY --from=build source/publish_output/ /app

RUN groupadd --system --gid 1000 app \
 && useradd --system --uid 1000 --gid app --shell /usr/sbin/nologin app \
 && chown -R app:app /app
USER app

ENTRYPOINT ["dotnet", "BooksApi.dll"]
