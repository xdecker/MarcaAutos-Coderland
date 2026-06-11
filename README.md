# MarcaAutos Coderland

API REST para administrar marcas de autos, construida con .NET 10, Entity Framework Core y PostgreSQL.

## Que incluye

- CRUD completo de marcas de autos (GET, POST, PUT, DELETE /api/marcasautos)
- Soft delete (los registros no se borran, solo se desactivan)
- Auditoria automatica (FechaCreacion, FechaActualizacion, FechaEliminacion)
- Data seeder que inserta Toyota, Ford y Volkswagen al iniciar si la tabla esta vacia
- Validacion de nombres duplicados (case-insensitive)
- Capa de servicios separada del controlador
- Pruebas unitarias con XUnit y base de datos en memoria (92% de cobertura)
- Docker Compose con PostgreSQL + API

## Requisitos

- .NET 10 SDK
- PostgreSQL 16 (si corres la API directo)
- Docker y Docker Compose (si usas contenedores)

## Como iniciar

### Opcion 1: Directo con la API

Primero asegurate de tener PostgreSQL corriendo en local y ajusta la cadena de conexion en `src/MarcaAutos.API/appsettings.json` si es necesario:

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=autos;Username=postgres;Password=tu_password"
}
```

Luego ejecuta las migraciones y levanta el proyecto:

```bash
dotnet ef database update --project src/MarcaAutos.API
dotnet run --project src/MarcaAutos.API
```

La API arranca en `http://localhost:5172` o el puerto configurado. El seeder se ejecuta automaticamente al iniciar y carga las 3 marcas si la tabla esta vacia. Si quieres desactivarlo, cambia `"SeedData": { "Enabled": false }` en el appsettings.

### Opcion 2: Con Docker Compose
* Para este metodo es necesario verificar que docker daemon se encuentre ejecutando.
* El compose tiene configurado usar el puerto 5435 para la DB.

Este metodo levanta PostgreSQL y la API juntos sin necesidad de tener nada instalado localmente (solo Docker):

```bash
docker compose up --build
```

La API arranca en `http://localhost:8080`. La base de datos se crea sola y el seeder se ejecuta automaticamente. Si quieres detenerlo:

```bash
docker compose down
```

Para borrar tambien el volumen de la base de datos:

```bash
docker compose down -v
```

## Endpoints

| Metodo | Ruta | Descripcion |
|--------|------|-------------|
| GET | /api/marcasautos | Lista todas las marcas activas |
| GET | /api/marcasautos/{id} | Obtiene una marca por ID |
| POST | /api/marcasautos | Crea una nueva marca |
| PUT | /api/marcasautos/{id} | Actualiza una marca existente |
| DELETE | /api/marcasautos/{id} | Desactiva una marca (soft delete) |

## Pruebas

Para ejecutar las pruebas:

```bash
dotnet test tests/MarcaAutos.Tests
```

Para ver el porcentaje de cobertura:

```bash
dotnet test tests/MarcaAutos.Tests --collect:"XPlat Code Coverage" --settings tests/MarcaAutos.Tests/coverlet.runsettings
```

El resultado de cobertura se guarda en `TestResults/` y el porcentaje global aparece en el archivo `coverage.cobertura.xml`.
