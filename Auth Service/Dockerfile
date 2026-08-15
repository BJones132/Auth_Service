FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /App

COPY . ./
RUN dotnet restore
RUN dotnet publish -o out

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /App
COPY --from=build /App/out .

EXPOSE 5000
EXPOSE 5010
VOLUME /home/App/.aspnet/https/

ENTRYPOINT ["dotnet", "Auth_Service.dll"]