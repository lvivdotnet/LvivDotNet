FROM node:10.15.3-alpine as build-stage
WORKDIR /src
COPY package*.json /src/
RUN yarn
COPY ./ /src/
RUN yarn build

FROM nginx:1.15.11

COPY --from=build-stage /build/ /usr/share/nginx/html
COPY --from=build-stage nginx.conf /etc/nginx/conf.d/default.conf