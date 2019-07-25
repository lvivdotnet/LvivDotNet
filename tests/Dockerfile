FROM mcr.microsoft.com/dotnet/core/sdk:2.2
RUN apt-get update
RUN apt-get install -y nginx
RUN rm -v /etc/nginx/nginx.conf
ADD nginx.conf /etc/nginx/
COPY . ./
RUN dotnet publish -c Release -o ./output
RUN ["chmod", "+x", "run-tests.sh"]
ENTRYPOINT "./run-tests.sh"
