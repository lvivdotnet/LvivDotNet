FROM mcr.microsoft.com/dotnet/core/sdk:2.2
COPY . ./
RUN dotnet build
EXPOSE 5000
ENTRYPOINT "dotnet" "test"