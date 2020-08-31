# Battleship game

Simple Battleship game implemented in C# and .net core 3.1.
The game uses console to render battlefield and comunicate with user.
You can run game or tests using .net core sdk or docker.

![Screen](/Assets/screen.jpg?raw=true "Screen")

## Running
#### .NET Core SDK 3.1
```
dotnet build Battleships/Battleships.csproj -c Release
dotnet run --project Battleships/Battleships.csproj
```
### Docker
```
docker build -f Battleships/Dockerfile -t battleship .
docker run -it battleship
```

## Tests
#### .NET Core SDK 3.1
```
dotnet build Battleships.Tests/Battleships.Tests.csproj -c Release
dotnet test Battleships.Tests/Battleships.Tests.csproj -c Release
```
#### Docker
```
docker build -f Battleships.Tests/Dockerfile -t battleship-tests .
docker run -it battleship-tests
```