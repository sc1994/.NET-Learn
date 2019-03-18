FROM microsoft/aspnetcore

COPY Socket/Demo  /app/Socket/

WORKDIR /app/Socket

ENTRYPOINT ["dotnet", "run"]