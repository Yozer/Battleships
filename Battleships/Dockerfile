FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["Battleships/Battleships.csproj", "Battleships/"]
RUN dotnet restore "Battleships/Battleships.csproj"
COPY . .
WORKDIR "/src/Battleships"
RUN dotnet build "Battleships.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Battleships.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/core/runtime:3.1 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Battleships.dll"]