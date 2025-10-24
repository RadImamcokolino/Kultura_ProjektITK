# slika, ki jo uporabimo za osnovo/strežnik

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

# delovni direktorij v sliki

WORKDIR /app

 

# vrata, ki jih želimo odpreti
EXPOSE 8080
EXPOSE 8081


 

# slika, ki jo uporabimo za izgradnjo

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

# kopiranje datotek v delovni direktorij

COPY ["Projekt_razvoj/Projekt_razvoj.csproj", "Projekt_razvoj/"]

# zagon ukaza za obnovo vseh odvisnosti

RUN dotnet restore "Projekt_razvoj/Projekt_razvoj.csproj"

COPY . .

WORKDIR "/src/Projekt_razvoj"

# zagon ukaza za izgradnjo projekta

RUN dotnet build "Projekt_razvoj.csproj" -c Release -o /app/build

 

FROM build AS publish

RUN dotnet publish "Projekt_razvoj.csproj" -c Release -o /app/publish /p:UseAppHost=false

# zagon ukaza za objavo projekta

 

FROM base AS final

WORKDIR /app

# kopiranje datotek iz začasne slike v končno

COPY --from=publish /app/publish .

# zagon ukaza za zagon aplikacije

ENTRYPOINT ["dotnet", "Projekt_razvoj.dll"]