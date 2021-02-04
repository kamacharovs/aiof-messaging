FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS build

# Copy .env variables through to Azure Function container
ENV AzureWebJobsStorage=$AzureWebJobsStorage
ENV ServiceBusConnectionString=$ServiceBusConnectionString
ENV DatabaseConnectionString=$DatabaseConnectionString
ENV StorageConnectionString=$StorageConnectionString
ENV EmailQueueName=$EmailQueueName
ENV EmailTableName=$EmailTableName
ENV InboundQueueName=$InboundQueueName
ENV InboundTableName=$InboundTableName

COPY . ./app

RUN cd /app/aiof.messaging.function && \
    mkdir -p /home/site/wwwroot && \
    dotnet publish *.csproj --output /home/site/wwwroot

# To enable ssh & remote debugging on app service change the base image to the one below
# FROM mcr.microsoft.com/azure-functions/dotnet:3.0-appservice
FROM mcr.microsoft.com/azure-functions/dotnet:3.0-slim
ENV AzureWebJobsScriptRoot=/home/site/wwwroot \
    AzureFunctionsJobHost__Logging__Console__IsEnabled=true

EXPOSE 80

COPY --from=build ["/home/site/wwwroot", "/home/site/wwwroot"]