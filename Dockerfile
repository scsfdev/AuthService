# AuthService/Dockerfile

# ---------- build stage ----------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# ---------- Build AuthService ----------
# Copy solution and project files for restore caching
COPY AuthService/AuthService.sln AuthService/
COPY AuthService/AuthService.Api/AuthService.Api.csproj AuthService/AuthService.Api/
COPY AuthService/AuthService.Application/AuthService.Application.csproj AuthService/AuthService.Application/
COPY AuthService/AuthService.Infrastructure/AuthService.Infrastructure.csproj AuthService/AuthService.Infrastructure/

# Restoring API will auto restore related dependencies.
RUN dotnet restore AuthService/AuthService.Api/AuthService.Api.csproj

# Copy rest of source
COPY AuthService/AuthService.Api/ AuthService/AuthService.Api/
COPY AuthService/AuthService.Application/ AuthService/AuthService.Application/
COPY AuthService/AuthService.Infrastructure/ AuthService/AuthService.Infrastructure/

# Publish
RUN dotnet publish AuthService/AuthService.Api/AuthService.Api.csproj -c Release -o /app --no-restore


# ---------- runtime stage ----------
# Final runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime

# 1. Add a non-root user named 'appuser'
RUN adduser --disabled-password --gecos "" --no-create-home appuser

WORKDIR /app

# 2. Change ownership of the /app directory to the new user.
RUN chown -R appuser:appuser /app

# 3. Copy published output
COPY --from=build /app ./

# 4. Expose the port.
EXPOSE 8080

# 5. Switch to the non-root user
USER appuser

# 6. Set the final entrypoint
ENTRYPOINT ["dotnet", "AuthService.Api.dll"]