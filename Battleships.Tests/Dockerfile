FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["Battleships.Tests/Battleships.Tests.csproj", "Battleships.Tests/"]
RUN dotnet restore "Battleships.Tests/Battleships.Tests.csproj"
COPY . .
WORKDIR "/src/Battleships.Tests"
RUN dotnet build "Battleships.Tests.csproj" -c Release -o /app/build

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS tests
WORKDIR /app
COPY --from=build /app/build .
ENTRYPOINT ["dotnet", "test", "Battleships.Tests.dll"]