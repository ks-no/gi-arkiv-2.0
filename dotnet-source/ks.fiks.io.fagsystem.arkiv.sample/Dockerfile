FROM mcr.microsoft.com/dotnet/sdk:3.1-alpine AS core-build-env
WORKDIR /build-app

# Copy csproj and build + publish code
COPY ks.fiks.io.arkivintegrasjon.client ./ks.fiks.io.arkivintegrasjon.client
COPY ks.fiks.io.arkivintegrasjon.common ./ks.fiks.io.arkivintegrasjon.common
COPY ks.fiks.io.arkivintegrasjon.sample.messages ./ks.fiks.io.arkivintegrasjon.sample.messages
COPY ks.fiks.io.fagsystem.arkiv.sample  ./ks.fiks.io.fagsystem.arkiv.sample 

RUN dotnet restore ks.fiks.io.fagsystem.arkiv.sample/ks.fiks.io.fagsystem.arkiv.sample.csproj
RUN dotnet build --configuration Release ks.fiks.io.fagsystem.arkiv.sample/ks.fiks.io.fagsystem.arkiv.sample.csproj
RUN dotnet publish --configuration Release ks.fiks.io.fagsystem.arkiv.sample/ks.fiks.io.fagsystem.arkiv.sample.csproj --no-build --output published-app

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:3.1
WORKDIR /app
COPY --from=core-build-env /build-app/published-app .

ENTRYPOINT ["dotnet", "ks.fiks.io.fagsystem.arkiv.sample.dll"]
