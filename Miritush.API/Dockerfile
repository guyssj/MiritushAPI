FROM mcr.microsoft.com/dotnet/sdk:6.0 as build

# RUN dotnet tool install -g dotnet-ef
# ENV PATH $PATH:/root/.dotnet/tools

WORKDIR /app
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o /app/published-app
# WORKDIR /app/Miritush.DAL
# RUN dotnet ef database update

FROM mcr.microsoft.com/dotnet/aspnet:6.0 as runtime
WORKDIR /app
COPY --from=build /app/published-app /app
ENTRYPOINT [ "dotnet", "/app/Miritush.API.dll" ]