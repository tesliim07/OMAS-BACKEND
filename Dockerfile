# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["OnlineMedicalAppointmentSystem.csproj", "./"]
RUN dotnet restore "OnlineMedicalAppointmentSystem.csproj"
COPY . .
RUN dotnet publish "OnlineMedicalAppointmentSystem.csproj" -c Release -o /app/publish


# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "OnlineMedicalAppointmentSystem.dll"]