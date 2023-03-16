FROM mcr.microsoft.com/dotnet/sdk:6.0 as build

# RUN dotnet tool install -g dotnet-ef
# ENV PATH $PATH:/root/.dotnet/tools

WORKDIR /app
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o /app/published-app
# WORKDIR /app/Miritush.DAL
# RUN dotnet ef database update

#COPY ./cert.pem /usr/local/share/ca-certificates/miritush.crt
#COPY ./key.pem /usr/local/share/ca-certificates/miritush.key
#RUN chmod 644 /usr/local/share/ca-certificates/miritush.crt \
 #   && chmod 400 /usr/local/share/ca-certificates/miritush.key \
  #  && update-ca-certificates

FROM mcr.microsoft.com/dotnet/aspnet:6.0 as runtime
WORKDIR /app
COPY --from=build /app/published-app /app

#ENV ASPNETCORE_URLS=https://+:443;http://+:80
#ENV ASPNETCORE_HTTPS_PORT=443
#ENV ASPNETCORE_Kestrel__Certificates__Default__Password=
#ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/https/key.pem
ENV ASPNETCORE_Kestrel__Certificates__Default__AllowInvalidCertificates=true

ENTRYPOINT [ "dotnet", "/app/Miritush.API.dll" ]
