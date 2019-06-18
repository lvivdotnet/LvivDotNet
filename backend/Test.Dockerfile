FROM mcr.microsoft.com/dotnet/core/sdk:2.2
COPY . ./
RUN dotnet build
ENTRYPOINT "dotnet" "test"