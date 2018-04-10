FROM microsoft/aspnetcore:2.0.5

COPY bin/Release/PublishOutput  /app/

WORKDIR /app

EXPOSE 7001

ENTRYPOINT ["dotnet", "/app/GRPC.Server.dll"]