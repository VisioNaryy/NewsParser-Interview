FROM mcr.microsoft.com/dotnet/sdk:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY NewsParser_Interview.sln ./
COPY NewsParser.API/*.csproj ./NewsParser.API/
COPY NewsParser.Application/*.csproj ./NewsParser.Application/
COPY NewsParser.Contracts/*.csproj ./NewsParser.Contracts/
COPY NewsParser.Domain/*.csproj ./NewsParser.Domain/
COPY NewsParser.Infrastructure/*.csproj ./NewsParser.Infrastructure/

RUN dotnet restore
COPY . .
WORKDIR /src/NewsParser.API
RUN dotnet build -c Release -o /app

WORKDIR /src/NewsParser.Application
RUN dotnet build -c Release -o /app

WORKDIR /src/NewsParser.Contracts
RUN dotnet build -c Release -o /app

WORKDIR /src/NewsParser.Domain
RUN dotnet build -c Release -o /app

WORKDIR /src/NewsParser.Infrastructure
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "NewsParser.API.dll"]