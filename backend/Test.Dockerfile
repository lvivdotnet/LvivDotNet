FROM mcr.microsoft.com/dotnet/core/sdk:2.2
COPY . ./
RUN dotnet build
RUN dotnet publish -c Release -o ./output
RUN ["chmod", "+x", "run-tests.sh"]
ENTRYPOINT "./run-tests.sh"
