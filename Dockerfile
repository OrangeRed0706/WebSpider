FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["WebSpider/WebSpider.csproj", "WebSpider/"]
RUN dotnet restore "WebSpider/WebSpider.csproj"
COPY . .
WORKDIR "/src/WebSpider"
RUN dotnet build "WebSpider.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebSpider.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
ENV TZ="Asia/Taipei"
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebSpider.dll"]

RUN apt-get update && apt-get install -y \
  curl \
  && rm -rf /var/lib/apt/lists/*