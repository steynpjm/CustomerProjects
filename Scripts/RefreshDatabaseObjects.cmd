
CD %~dp0
PUSHD %~dp0
CD ..\Database\SteynPJM.CustomerProjects.DatabaseLibrary

dotnet ef dbcontext scaffold --schema dbo --startup-project ..\helperconsole\helperconsole.csproj "Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=CustomerProjects;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False" Microsoft.EntityFrameworkCore.SqlServer --context BaseDataContext --force --no-pluralize --no-onconfiguring

POPD
