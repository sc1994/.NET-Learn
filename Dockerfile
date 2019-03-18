FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["Socket/Demo/Demo.csproj", "Demo/"]
RUN dotnet restore "Demo/Demo.csproj"
COPY . .
WORKDIR "/Demo"
RUN dotnet build "Demo.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Demo.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Demo.dll"]