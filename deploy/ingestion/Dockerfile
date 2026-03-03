# Estágio de Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia da raiz do repositório direto para o /src do container
COPY ["AgroSolutions-IngestionService.csproj", "./"]
RUN dotnet restore "AgroSolutions-IngestionService.csproj"

# Copia o resto do código
COPY . . 
RUN dotnet publish "AgroSolutions-IngestionService.csproj" -c Release -o /app/publish

# Estágio de Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "AgroSolutions-IngestionService.dll"]