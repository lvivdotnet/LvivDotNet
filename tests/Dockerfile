FROM mcr.microsoft.com/dotnet/core/sdk:2.2
COPY . ./
RUN dotnet publish -c Release -o ./output -v n
ENTRYPOINT "dotnet" "./LvivDotNet.Tests/output/LvivDotNet.Tests.dll"