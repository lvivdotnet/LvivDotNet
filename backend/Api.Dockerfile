FROM mcr.microsoft.com/dotnet/core/sdk:2.2 as build-stage
COPY . ./
ENV ASPNETCORE_URLS http://*:5000
RUN dotnet publish -c Release -o ./output -v n
EXPOSE 5000
ENTRYPOINT "dotnet" "./Lviv .NET Platform.WebApi/output/Lviv .NET Platform.WebApi.dll"
