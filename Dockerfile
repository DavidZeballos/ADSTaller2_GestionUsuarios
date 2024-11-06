# Usa la imagen base de .NET SDK para construir la aplicación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copia el archivo de solución y el archivo de proyecto primero para restaurar las dependencias
COPY UserManagementService.sln ./
COPY src/*.csproj ./src/

# Restaura las dependencias
RUN dotnet restore ./src/UserManagementService.csproj

# Copia el resto de los archivos del proyecto
COPY . .

# Publica la aplicación en modo Release
RUN dotnet publish ./src/UserManagementService.csproj -c Release -o out

# Usa una imagen de .NET Runtime para ejecutar la aplicación
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .

# Exponer el puerto en el que la aplicación escucha (ajústalo si es necesario)
EXPOSE 5000

# Ejecuta la aplicación
ENTRYPOINT ["dotnet", "UserManagementService.dll"]
