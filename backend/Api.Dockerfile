FROM mcr.microsoft.com/dotnet/core/sdk:2.2
COPY . ./
RUN dotnet build
RUN dotnet publish -c Release -o ./output
ENTRYPOINT "dotnet" "./LvivDotNet.WebApi/output/LvivDotNet.WebApi.dll"
