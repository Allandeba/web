#FROM mcr.microsoft.com/dotnet/runtime:7.0
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY ./publish .

ENTRYPOINT [ "dotnet", "getQuote.dll" ]