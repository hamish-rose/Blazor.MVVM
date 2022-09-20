FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Blazor.MVVM/Blazor.MVVM.csproj", "Blazor.MVVM/"]
RUN dotnet restore "Blazor.MVVM/Blazor.MVVM.csproj"
COPY . .
WORKDIR "/src/Blazor.MVVM"
RUN dotnet build "Blazor.MVVM.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Blazor.MVVM.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Blazor.MVVM.dll"]
