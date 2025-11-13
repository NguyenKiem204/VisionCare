# VisionCare Backend (ASP.NET Core)
3s!8BDa4QG@hgxe
## Run with Docker

- Build and start (from backend root):
- docker-compose down && docker-compose build api && docker-compose up -d

```
docker compose up --build -d
```

- Stop and remove containers:

```
docker compose down
```

- Reset database (re-run init SQL):

```
docker compose down -v && docker compose up --build -d
```

- Follow logs:

```
docker compose logs -f api
```

## Database & pgAdmin

- pgAdmin URL: http://localhost:8080
- pgAdmin login:
  - Email: admin@admin.com
  - Password: admin123

Register Server in pgAdmin:

- General → Name: VisionCare DB
- Connection:
  - Host name/address: db
  - Port: 5432
  - Maintenance database: visioncare
  - Username: postgres
  - Password: postgres123
  - Save password: Yes

Connect from local tools (DBeaver/psql):

- Host: localhost
- Port: 5433
- Database: visioncare
- Username: postgres
- Password: postgres123

## Configuration

- App settings: src/WebAPI/appsettings.json
- Docker env via docker-compose.yml
- Optional .env (not committed):

```
POSTGRES_DB=visioncare
POSTGRES_USER=postgres
POSTGRES_PASSWORD=postgres123
ASPNETCORE_ENVIRONMENT=Development
API_PORT=5000
DB_PORT=5433
PGADMIN_DEFAULT_EMAIL=admin@admin.com
PGADMIN_DEFAULT_PASSWORD=admin123
```

## Notes

- Init SQL runs from docker/db/init on first DB creation only.
- To re-seed, use: docker compose down -v before starting again.
