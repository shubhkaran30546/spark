# Use the official .NET SDK image as a build environment 
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build 
# Set the working directory inside the container to /src 
WORKDIR /src 
# Copy the .csproj file to the working directory 
COPY ["part3.csproj", "./"] 
# Restore the dependencies for the project 
RUN dotnet restore "part3.csproj" 
# Copy the entire project to the working directory 
COPY . . 
# Build the project 
COPY wwwroot/images /app/wwwroot/images
RUN dotnet build "part3.csproj" -c Release -o /app/build 
# Publish the project to a folder 
RUN dotnet publish "part3.csproj" -c Release -o /app/publish 
# Use the official .NET runtime image as a base image 
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime 
# Set the working directory to /app 
WORKDIR /app 
# Copy the published output from the build stage to the runtime stage 
COPY --from=build /app/publish . 
# Define the entry point for the application 
ENTRYPOINT ["dotnet", "part3.dll"]
