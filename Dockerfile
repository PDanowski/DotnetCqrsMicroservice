########################################
#  Use Build image 
########################################
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS builder

# Setup working directory for project
WORKDIR /app

COPY ./.editorconfig ./
COPY ./Directory.Build.props ./
COPY ./Core.Build.props ./
COPY ./DeviceService/DeviceService.csproj ./DeviceService/
COPY ./DeviceService.Api/DeviceService.Api.csproj ./DeviceService.Api/

# Restore nuget packages
RUN dotnet restore ./DeviceService.Api/DeviceService.Api.csproj

# Copy project files
COPY ./DeviceService ./DeviceService
COPY ./DeviceService.Api/ ./DeviceService.Api

# Build project with Release configuration and no restore
RUN dotnet build -c Release --no-restore ./DeviceService.Api/DeviceService.Api.csproj

## Test project with Release configuration and no build
RUN dotnet test -c Release --no-build ./DeviceService.Api/DeviceService.Api.Tests.csproj

# Publish project to output folder and no build
WORKDIR /app/DeviceService.Api
RUN ls
RUN dotnet publish -c Release --no-build -o out

########################################
#  Use other build image as the final one that won't have source codes
########################################
FROM mcr.microsoft.com/dotnet/sdk:7.0

# Setup working directory for project
WORKDIR /app

# Copy published in previous stage binaries from the `builder` image
COPY --from=builder /app/DeviceService.Api/out .

# Set URL that App will be exposed
ENV ASPNETCORE_URLS="http://*:5000"

# Sets entry point command to automatically run application on `docker run`
ENTRYPOINT ["dotnet", "DeviceService.Api.dll"]