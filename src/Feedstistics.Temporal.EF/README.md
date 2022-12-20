-- Build Migrations project and drop binaries in EF project bin
dotnet build ..\Feedstistics.EF.Migrations\Feedstistics.EF.Migrations.csproj -o .\bin\Debug\net7.0\

-- Create a Migration
dotnet build -o .\bin\Debug\net7.0\
dotnet ef migrations add %MyMigrationName% --project ..\Feedstistics.EF.Migrations --output-dir Migrations
-- NOTE: Review migration created before checking in. Add any Transformation or SEED data in that migration if required

-- Create efBundles.exe file
dotnet build ..\Feedstistics.EF.Migrations\Feedstistics.EF.Migrations.csproj -o .\bin\Debug\net7.0\
dotnet ef migrations bundle --project ..\Feedstistics.EF.Migrations --verbose --force

-- Local deploy
.\efBundle.exe

-- Custom connection string deploy
.\efBundle.exe --connect {$ENVVARWITHCONNECTION}