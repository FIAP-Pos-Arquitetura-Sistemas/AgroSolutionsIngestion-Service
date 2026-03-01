FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["../../AgroSolutions-IngestionService.csproj", "./"]
RUN dotnet restore "AgroSolutions-IngestionService.csproj"
COPY "../../" . 
RUN dotnet publish "AgroSolutions-IngestionService.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
# O pulo do gato: passa o argumento para o Program.cs
ENTRYPOINT ["dotnet", "AgroSolutions-IngestionService.dll", "--run-worker"]