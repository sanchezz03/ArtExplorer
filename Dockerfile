FROM mcr.microsoft.com/dotnet/sdk:9.0.200 AS build
WORKDIR /app

COPY . ./

RUN dotnet restore

EXPOSE 7257
EXPOSE 5275
CMD ["dotnet", "run", "--project", "ArtExplorer.API"]
