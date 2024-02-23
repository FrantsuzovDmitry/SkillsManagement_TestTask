0. git clone
1. Открыть папку в командной строке
2. dotnet ef migrations add InitialCreate
3. dotnet ef database update
4. Запустить приложение на нужном посту:
   dotnet run --urls="https://localhost:{port}"

В качестве БД используется SqlServer.




