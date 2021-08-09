# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /app

COPY Teller.Api/ ./Teller.Api/
COPY Teller.Logic/ ./Teller.Logic/
COPY Teller.ServiceInterfaces/ ./Teller.ServiceInterfaces/
COPY Tests/ ./Tests/
COPY SelfServiceCheckout.sln ./
RUN ls -l
RUN dotnet restore

# Build
RUN dotnet publish -c Debug -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:5.0
EXPOSE 5000
EXPOSE 5001
EXPOSE 80
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "Teller.Api.dll", "--urls", "http://localhost:5000,https://localhost:5001"]
