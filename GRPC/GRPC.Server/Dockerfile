FROM microsoft/aspnetcore:2.0.5

COPY GRPC\GRPC.Server\bin\Release\netcoreapp2.0  /app/

WORKDIR /app

EXPOSE 7003

ENTRYPOINT ["dotnet", "GRPC.Server.dll"]