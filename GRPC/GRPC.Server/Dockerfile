FROM microsoft/aspnetcore:2.0.5

COPY /home/ubuntu/.NET-Learn/GRPC/GRPC.Server/bin/Release/netcoreapp2.0  /app/

WORKDIR /app

EXPOSE 7001

ENTRYPOINT ["dotnet", "GRPC.Server.dll"]