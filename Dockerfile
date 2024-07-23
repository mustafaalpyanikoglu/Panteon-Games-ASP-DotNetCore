# Base image olarak .NET 8 SDK'sýný kullanýn
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Çalýþma dizinini belirleyin
WORKDIR /app

# Çözüm dosyasýný ve proje dosyalarýný kopyalayýn
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

# Tüm dosyalarý kopyalayýn
COPY . ./

# Çözümü restore edin
RUN dotnet restore WebAPI.sln

# Çözümü publish edin
RUN dotnet publish WebAPI.sln -c Release -o /app/out

# Koþulacak temel imajý ayarlayýn
FROM mcr.microsoft.com/dotnet/aspnet:8.0

# Çalýþma dizinini belirleyin
WORKDIR /app

# Yayýnlanan uygulama dosyalarýný kopyalayýn
COPY --from=build /app/out .

# Uygulamanýn hangi portu dinleyeceðini ayarlayýn
ENV ASPNETCORE_URLS=http://+:8080

# Uygulamayý baþlatýn
ENTRYPOINT ["dotnet", "WebAPI.dll"]
