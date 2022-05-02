# Ray.Blog

## Add Migrate

Add-Migration "Initial" -OutputDir "Migrations" -Context BlogDbContext

Add-Migration "Initial" -OutputDir "SecondDbMigrations" -Context BlogSecondDbContext

Add-Migration "Initial" -OutputDir "BlobDbMigrations" -Context BlogBlobDbContext
