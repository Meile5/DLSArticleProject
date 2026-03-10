dotnet ef dbcontext scaffold "Server=localhost,1442;Database=DraftServiceDb;User Id=sa;Password=SuperSecret7!;TrustServerCertificate=True;" `
  Microsoft.EntityFrameworkCore.SqlServer `
  --output-dir Models `
  --context-dir Database `
  --context DraftContext `
  --no-onconfiguring `
  --force