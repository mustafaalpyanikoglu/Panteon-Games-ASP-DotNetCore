# Base image olarak .NET 8 SDK's�n� kullan�n
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# �al��ma dizinini belirleyin
WORKDIR /app

# ��z�m dosyas�n� ve proje dosyalar�n� kopyalay�n
COPY WebAPI.sln ./
COPY Application/*.csproj ./Application/
COPY core/Core.Application/*.csproj ./core/Core.Application/
COPY core/Core.CrossCuttingConcerns/*.csproj ./core/Core.CrossCuttingConcerns/
COPY core/Core.Persistence/*.csproj ./core/Core.Persistence/
COPY core/Core.Security/*.csproj ./core/Core.Security/
COPY Domain/*.csproj ./Domain/
COPY Infrastructure/*.csproj ./Infrastructure/
COPY Persistence/*.csproj ./Persistence/
COPY WebAPI/*.csproj ./WebAPI/

# T�m dosyalar� kopyalay�n
COPY . ./

# ��z�m� restore edin
RUN dotnet restore WebAPI.sln

# ��z�m� publish edin
RUN dotnet publish WebAPI.sln -c Release -o /app/out

# Ko�ulacak temel imaj� ayarlay�n
FROM mcr.microsoft.com/dotnet/aspnet:8.0

# �al��ma dizinini belirleyin
WORKDIR /app

# Yay�nlanan uygulama dosyalar�n� kopyalay�n
COPY --from=build /app/out .

# Uygulaman�n hangi portu dinleyece�ini ayarlay�n
ENV ASPNETCORE_URLS=http://+:8080

# Uygulamay� ba�lat�n
ENTRYPOINT ["dotnet", "WebAPI.dll"]
