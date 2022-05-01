# Ray.Blog

## Migrate Db

Add-Migration "Initial" -OutputDir "Migrations" -Context BlogDbContext

Add-Migration "Initial" -OutputDir "SecondDbMigrations" -Context BlogSecondDbContext
