FROM microsoft/aspnetcore:2.0.5

COPY bin/Release/PublishOutput  /app/

WORKDIR /app

EXPOSE 7002

ENTRYPOINT ["dotnet", "DockerDemo1.dll"]