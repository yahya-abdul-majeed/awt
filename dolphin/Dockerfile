FROM node as build 
WORKDIR /src
COPY . . 
RUN npm install
RUN npm run build

FROM nginx as release 
WORKDIR /usr/share/nginx/html
RUN rm -rf *
COPY --from=build /src/build .


ENTRYPOINT ["nginx","-g","daemon off;"]
